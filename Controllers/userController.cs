using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using testeBitzen.Data;
using testeBitzen.Services;
using System.Text.Json;
using testeBitzen.Models;
using System.Security.Claims;
using System;

namespace testeBitzen.Controllers
{
    [Route("v1/user")]
    [AllowAnonymous]
    public class userController : ControllerBase
    {
        [HttpPost]
        [Route("auth")]
        public async Task<ActionResult<dynamic>> Auth([FromServices] DataContext context, [FromBody] User model)
        {
            try
            {
                string senha = Services.EncriptService.GenerateEcriptionSHA256(model.Senha);
                User user = await context.Users.Where(u => u.Email == model.Email && u.Senha == senha).FirstOrDefaultAsync();
                if (user == null)
                    return NotFound(new { message = "Usuário não encontrado" });
                user.Senha = Services.EncriptService.GenerateEcriptionSHA256(model.Senha); ;
                var token = TokenService.GenerateToken(user);
                user.Senha = "";
                return new
                {
                    user = user,
                    token = token
                };
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message, Inner = ex.InnerException.Message.ToString() });
            }
        }
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<dynamic>> Gravar([FromServices] DataContext context, [FromBody] User model)
        {

            if (ModelState.IsValid)
            {
                User user = await context.Users.Where(u => u.Email == model.Email).FirstOrDefaultAsync();
                if (user == null)
                {
                    try
                    {
                        model.Senha = Services.EncriptService.GenerateEcriptionSHA256(model.Senha);
                        context.Users.Add(model);
                        await context.SaveChangesAsync();
                        model.Senha = "";
                        return Created("", model);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new { Message = ex.Message, Inner = ex.InnerException.Message.ToString() });
                    }

                }
                else
                    return BadRequest(new { Email = "Email já cadastrado" });

            }
            else
                return BadRequest(ModelState);
        }
        [HttpPut]
        [Route("")]
        public async Task<ActionResult<dynamic>> Update([FromServices] DataContext context, [FromBody] User model)
        {
            User user;
            Console.WriteLine(model.Id);
            if (model.Id == 0 || model.Senha == "" || model.Email == "")
                return BadRequest(model);

            if (ModelState.IsValid)
            {
                user = await context.Users.Where(u => u.Email == model.Email && u.Id!= model.Id).FirstOrDefaultAsync();
                if (user == null)
                {
                    user = await context.Users.FirstOrDefaultAsync(x => x.Id == model.Id);
                    if (user != null)
                    {
                        try
                        {
                            var email = User.Claims.Where(c => c.Type == ClaimTypes.Email)
                            .Select(c => c.Value).SingleOrDefault();
                            if (user.Email != email)
                                return BadRequest(new { message = "Você deve estar logado no usuario apra alterar-lo" });
                            context.Entry(user).State = EntityState.Detached;
                            model.Senha = Services.EncriptService.GenerateEcriptionSHA256(model.Senha);
                            context.Users.Update(model);
                            await context.SaveChangesAsync();
                            model.Senha = "";
                            return model;
                        }
                        catch (Exception ex)
                        {
                            return BadRequest(new { Message = ex.Message, Inner = ex.InnerException.Message.ToString() });
                        }
                    }
                    else
                        return
                        NotFound(model);
                }
                return BadRequest(new { Email = "Email já cadastrado" });
            }
            else
                return BadRequest(ModelState);
        }
        [HttpDelete]
        [Route("")]
        public async Task<ActionResult<dynamic>> Delete([FromServices] DataContext context, [FromBody] User model)
        {
            User user;
            if (model.Id == 0)
                return BadRequest(model);

            user = await context.Users.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (user != null)
            {
                try
                {
                    var email = User.Claims.Where(c => c.Type == ClaimTypes.Email)
                   .Select(c => c.Value).SingleOrDefault();
                    if (user.Email != email)
                        return BadRequest(new { message = "Você deve estar logado no usuario apra excluir-lo" });
                    context.Entry(user).State = EntityState.Detached;
                    context.Users.Remove(model);
                    await context.SaveChangesAsync();
                    return Ok(new { Message = "deletado com sucesso" });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = ex.Message, Inner = ex.InnerException.Message.ToString() });
                }
            }
            else
                return NotFound(model);
        }
    }
}