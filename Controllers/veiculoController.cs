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
    [Route("v1/veiculo")]
    public class veiculoController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<List<Veiculo>>> Get([FromServices] DataContext context)
        {
            var veiculo = await context.Veiculos.Include(x => x.Responsavel).AsNoTracking().ToListAsync();
            return veiculo;
        }
        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Gravar([FromServices] DataContext context, [FromBody] Veiculo model)
        {


            if (ModelState.IsValid)
            {
                User user = await context.Users.Where(x => x.Id == model.ResponsavelId).SingleOrDefaultAsync();
                if (user != null)
                {
                    context.Entry(user).State = EntityState.Detached;

                    context.Veiculos.Add(model);
                    await context.SaveChangesAsync();

                    return Created("", model);
                }
                else
                    return BadRequest(new { ResponsavelId = "Usuario não existe" });

            }
            else
                return BadRequest(ModelState);
        }
        [HttpPut]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Update([FromServices] DataContext context, [FromBody] Veiculo model)
        {

            if (model.Id == 0)
                BadRequest(model);
            if (ModelState.IsValid)
            {
                var email = User.Claims.Where(c => c.Type == ClaimTypes.Email)
                   .Select(c => c.Value).SingleOrDefault();
            
                var veiculo = await context.Veiculos.Include(re => re.Responsavel)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == model.Id && x.Responsavel.Email == email);
                if (veiculo != null)
                {
                    User user = await context.Users.Where(x => x.Id == model.ResponsavelId).SingleOrDefaultAsync();
                    if (user != null)
                    {
                        context.Entry(veiculo).State = EntityState.Detached;
                        context.Veiculos.Update(model);
                        await context.SaveChangesAsync();
                        return model;
                    }
                    else
                        return BadRequest(new { ResponsavelId = "Usuario não existe" });

                }
                else
                    return
                    NotFound(model);
            }
            else
                return BadRequest(ModelState);
        }
        [HttpDelete]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Delete([FromServices] DataContext context, [FromBody] Veiculo model)
        {
            if (model.Id == 0)
                return BadRequest(new { ID = model.Id });
            var email = User.Claims.Where(c => c.Type == ClaimTypes.Email)
                   .Select(c => c.Value).SingleOrDefault();
            var veiculo = await context.Veiculos.Include(re => re.Responsavel)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.Responsavel.Email == email);
            if (veiculo != null)
            {
                context.Entry(veiculo).State = EntityState.Detached;
                context.Veiculos.Remove(model);
                await context.SaveChangesAsync();
                return Ok(new { Message = "deletado com sucesso" });
            }
            else
                return NotFound(new { ID = model.Id });
        }



    }
}