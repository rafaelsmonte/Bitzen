using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testeBitzen.Data;
using testeBitzen.Models;

namespace testeBitzen.Controllers
{
    [ApiController]
    [Route("v1/abastecimento")]
    public class abastecimentoController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Abastecimento>>> Get([FromServices] DataContext context)
        {
            var Abastecimento = await context.Abastecimentos.ToListAsync();
            return Abastecimento;
        }
    }
}