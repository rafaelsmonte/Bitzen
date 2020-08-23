using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testeBitzen.Data;
using testeBitzen.Models;

namespace testeBitzen.Controllers
{

    [Route("v1/abastecimento")]
    public class abastecimentoController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [Authorize]

        public async Task<ActionResult<List<Abastecimento>>> Get([FromServices] DataContext context)
        {
            try
            {
                var email = User.Claims.Where(c => c.Type == ClaimTypes.Email)
                       .Select(c => c.Value).SingleOrDefault();
                var Abastecimento = await context.Abastecimentos.Include(x => x.Responsavel).Where(x => x.Responsavel.Email == email).ToListAsync();


                return Abastecimento;
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message, Inner = ex.InnerException.Message.ToString() });
            }
        }
        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Gravar([FromServices] DataContext context, [FromBody] Abastecimento model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = await context.Users.Where(x => x.Id == model.ResponsavelId).SingleOrDefaultAsync();
                    if (user != null)
                    {
                        var email = User.Claims.Where(c => c.Type == ClaimTypes.Email)
                       .Select(c => c.Value).SingleOrDefault();
                        var veiculo = await context.Veiculos.Include(re => re.Responsavel)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == model.VeiculoId && x.Responsavel.Email == email);
                        if (veiculo != null)
                        {
                            var ultimoAbastecimento = await context.Abastecimentos
                            .Where(x => x.VeiculoId == model.VeiculoId)
                            .OrderByDescending(x => x.DataDoAbastecimento)
                            .FirstOrDefaultAsync();
                            if (ultimoAbastecimento != null && ultimoAbastecimento.KmDoAbastecimento >= model.KmDoAbastecimento)
                                return BadRequest(new { ResponsavelId = "Quilometragem menor do que a registrada no ultimo abastecimento" });




                            context.Abastecimentos.Add(model);
                            await context.SaveChangesAsync();
                            model.Responsavel.Senha = "";

                            return Created("", model);
                        }
                        else
                            return BadRequest(new { VeiculoId = "Veiculo não existe" });
                    }
                    else
                        return BadRequest(new { ResponsavelId = "User não existe" });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = ex.Message, Inner = ex.InnerException.Message.ToString() });
                }
            }
            else
                return BadRequest(ModelState);
        }
        [HttpPut]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Update([FromServices] DataContext context, [FromBody] Abastecimento model)
        {
            if (model.Id == 0)
                return BadRequest(new { ID = model.Id });
            if (ModelState.IsValid)
            {
                try
                {
                    User user = await context.Users.Where(x => x.Id == model.ResponsavelId).SingleOrDefaultAsync();
                    if (user != null)
                    {
                        var email = User.Claims.Where(c => c.Type == ClaimTypes.Email)
                       .Select(c => c.Value).SingleOrDefault();
                        var veiculo = await context.Veiculos.Include(re => re.Responsavel)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == model.VeiculoId && x.Responsavel.Email == email);
                        if (veiculo != null)
                        {
                            model.DataDoAbastecimento = DateTime.Now;
                            context.Abastecimentos.Update(model);
                            await context.SaveChangesAsync();
                            model.Responsavel.Senha = "";
                            return Created("", model);
                        }
                        else
                            return BadRequest(new { VeiculoId = "Veiculo não existe" });
                    }
                    else
                        return BadRequest(new { ResponsavelId = "User não existe" });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = ex.Message, Inner = ex.InnerException.Message.ToString() });
                }
            }
            else
                return BadRequest(ModelState);
        }
        [HttpDelete]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Delete([FromServices] DataContext context, [FromBody] Abastecimento model)
        {
            if (model.Id == 0)
                return BadRequest(new { ID = model.Id });
            try
            {
                var email = User.Claims.Where(c => c.Type == ClaimTypes.Email)
                       .Select(c => c.Value).SingleOrDefault();

                var abastecimento = await context.Abastecimentos.Include(re => re.Responsavel)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == model.Id && x.Responsavel.Email == email);
                if (abastecimento != null)
                {
                    context.Abastecimentos.Remove(model);
                    await context.SaveChangesAsync();

                    return Ok(new { Message = "deletado com sucesso" });
                }
                else
                    return NotFound(new { ID = model.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message, Inner = ex.InnerException.Message.ToString() });
            }
        }
    }
}