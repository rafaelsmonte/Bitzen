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
            string senha = Services.EncriptService.GenerateEcriptionSHA256(model.Senha);
            User user = await context.Users.Where(u => u.Email == model.Email && u.Senha == senha).FirstOrDefaultAsync();
            if (user == null)
                return NotFound(new { message = "Usuário não encontrado" });
            var token = TokenService.GenerateToken(user);
            user.Senha = "";
            return new
            {
                user = user,
                token = token
            };
        }
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<dynamic>> Gravar([FromServices] DataContext context, [FromBody] User model)
        {

            if (ModelState.IsValid)
            {
                model.Senha = Services.EncriptService.GenerateEcriptionSHA256(model.Senha);
                context.Users.Add(model);
                await context.SaveChangesAsync();
                model.Senha = "";

                return Created("",model);
            }
            else
                return BadRequest(ModelState);
        }
        [HttpPut]
        [Route("")]
        public async Task<ActionResult<dynamic>> Update([FromServices] DataContext context, [FromBody] User model)
        {
            User user;

            if (model.Id == 0)
                NotFound(model);

            if (ModelState.IsValid)
            {
                user = await context.Users.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (user != null)
                {
                    context.Entry(user).State = EntityState.Detached;
                    model.Senha = Services.EncriptService.GenerateEcriptionSHA256(model.Senha);
                    context.Users.Update(model);
                    await context.SaveChangesAsync();
                    model.Senha = "";
                    return model;
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
        public async Task<ActionResult<dynamic>> Delete([FromServices] DataContext context, [FromBody] User model)
        {
            User user;
            if (model.Id == 0)
                NotFound(model);
            user = await context.Users.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (user != null)
            {
                context.Entry(user).State = EntityState.Detached;
                context.Users.Remove(model);
                await context.SaveChangesAsync();
                return Ok(new {Message=  "deletado com sucesso"});
            }
            else
                return NotFound(model);
        }
    }
}