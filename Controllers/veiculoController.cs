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
    [Route("v1/veiculo")]
    public class veiculoController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Veiculo>>> Get([FromServices] DataContext context)
        {
            var veiculo = await context.Veiculos.ToListAsync();
            return veiculo;
        }
    }
}