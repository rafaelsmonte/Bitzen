using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testeBitzen.Data;
using testeBitzen.Services;
using testeBitzen.Models;

namespace testeBitzen.Controllers
{

    [Route("v1/relatorios")]
    public class relatoriosController : ControllerBase
    {
        [HttpGet]
        [Route("AbastecidoPorAno/{ano:int}")]
        [Authorize]

        public async Task<dynamic> QuantidadeAbastecidaPorVeiculoPorAno(int ano, [FromServices] DataContext context)
        {
            try
            {
                var email = User.Claims.Where(c => c.Type == ClaimTypes.Email)
                       .Select(c => c.Value).SingleOrDefault();
                var Abastecimento = await context.Abastecimentos
                .Include(r => r.Responsavel)
                .Include(v => v.Veiculo)
                .Where(x => x.Responsavel.Email == email && x.DataDoAbastecimento.Year == ano)
                .GroupBy(x => new { mes = x.DataDoAbastecimento.Month, VeiculoID = x.VeiculoId })
                .Select(x => new
                {
                    Mes = Services.Meses.getNomeMes(x.Key.mes),
                    Veiculo = x.Key.VeiculoID,
                    TotalDeLitros = x.Sum(i => i.LitrosAbastecidos),

                })
                .ToListAsync();
                return Abastecimento;
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message, Inner = ex.InnerException.Message.ToString() });
            }
        }
        [HttpGet]
        [Route("PagoPorAno/{ano:int}")]
        [Authorize]

        public async Task<dynamic> QuantidadePagaPorAnoPorVeiculo(int ano, [FromServices] DataContext context)
        {
            try
            {
                var email = User.Claims.Where(c => c.Type == ClaimTypes.Email)
                       .Select(c => c.Value).SingleOrDefault();
                var Abastecimento = await context.Abastecimentos
                .Include(r => r.Responsavel)
                .Include(v => v.Veiculo)
                .Where(x => x.Responsavel.Email == email && x.DataDoAbastecimento.Year == ano)
                .GroupBy(x => new { mes = x.DataDoAbastecimento.Month, VeiculoID = x.VeiculoId })
                .Select(x => new
                {
                    Mes = Services.Meses.getNomeMes(x.Key.mes),
                    Veiculo = x.Key.VeiculoID,
                    TotalPago = x.Sum(i => i.ValorPago),

                })
                .ToListAsync();
                return Abastecimento;
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message, Inner = ex.InnerException.Message.ToString() });
            }
        }
        [HttpGet]
        [Route("KmRodadoPorAno/{ano:int}")]
        [Authorize]
        public async Task<dynamic> KmRodadoPorMes(int ano, [FromServices] DataContext context)
        {
            try
            {
                var email = User.Claims.Where(c => c.Type == ClaimTypes.Email)
                       .Select(c => c.Value).SingleOrDefault();
                var Abastecimentos = await context.Abastecimentos
                .Include(r => r.Responsavel)
                .Include(v => v.Veiculo)
                .Where(x => x.Responsavel.Email == email && x.DataDoAbastecimento.Year == ano)
                .GroupBy(x => new { Mes = x.DataDoAbastecimento.Month, Data = x.DataDoAbastecimento, KM = x.KmDoAbastecimento })
                .OrderBy(x => x.Key.Data)
                .Select(x => new
                {
                    Mes = x.Key.Mes,
                    Data = x.Key.Data,
                    KM = x.Key.KM,
                })
                .ToListAsync();
                List<dynamic> ret = new List<dynamic>();
                foreach (var Abastecimento in Abastecimentos)
                {
                    var maiorData = Abastecimentos
                    .Where(x => x.Mes == Abastecimento.Mes)
                    .OrderByDescending(c => c.Data)
                    .FirstOrDefault();

                    var menorData = Abastecimentos
                    .Where(x => x.Mes == Abastecimento.Mes)
                    .OrderBy(c => c.Data)
                    .FirstOrDefault();
                    ret.Add(new { Mes = Abastecimento.Mes, KMRodados = maiorData.KM - menorData.KM });

                }

                return ret.GroupBy(x => new { Mes = Services.Meses.getNomeMes(x.Mes), KMRodados = x.KMRodados }).Select(x => new { Mes = x.Key.Mes, KMRodados = x.Key.KMRodados });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message, Inner = ex.InnerException.Message.ToString() });
            }
        }
        [HttpGet]
        [Route("MediaMensal/{ano:int}")]
        [Authorize]
        public async Task<dynamic> MediaMensalCarro(int ano, [FromServices] DataContext context)
        {
            try
            {
                var email = User.Claims.Where(c => c.Type == ClaimTypes.Email)
                       .Select(c => c.Value).SingleOrDefault();
                var Abastecimentos = await context.Abastecimentos
                .Include(r => r.Responsavel)
                .Include(v => v.Veiculo)
                .Where(x => x.Responsavel.Email == email && x.DataDoAbastecimento.Year == ano)
                .GroupBy(x => new { Mes = x.DataDoAbastecimento.Month, Data = x.DataDoAbastecimento, KM = x.KmDoAbastecimento })
                .OrderBy(x => x.Key.Data)
                .Select(x => new
                {
                    Mes = x.Key.Mes,
                    Data = x.Key.Data,
                    KM = x.Key.KM,
                })
                .ToListAsync();
                List<dynamic> ret = new List<dynamic>();
                foreach (var Abastecimento in Abastecimentos)
                {
                    var maiorData = Abastecimentos
                    .Where(x => x.Mes == Abastecimento.Mes)
                    .OrderByDescending(c => c.Data)
                    .FirstOrDefault();

                    var menorData = Abastecimentos
                    .Where(x => x.Mes == Abastecimento.Mes)
                    .OrderBy(c => c.Data)
                    .FirstOrDefault();
                    ret.Add(new { Mes = Abastecimento.Mes, KMRodados = maiorData.KM - menorData.KM });

                }

                var KMRodadosPorMes = ret.GroupBy(x => new { Mes = Services.Meses.getNomeMes(x.Mes), KMRodados = x.KMRodados }).Select(x => new { Mes = x.Key.Mes, KMRodados = x.Key.KMRodados });

                var LitrosPorMes = await context.Abastecimentos
                .Include(r => r.Responsavel)
                .Include(v => v.Veiculo)
                .Where(x => x.Responsavel.Email == email && x.DataDoAbastecimento.Year == ano)
                .GroupBy(x => new { mes = x.DataDoAbastecimento.Month })
                .OrderBy(x => x.Key.mes)
                .Select(x => new
                {
                    Mes = Services.Meses.getNomeMes(x.Key.mes),
                    Total = x.Sum(i => i.LitrosAbastecidos),

                })
                .ToListAsync();

                List<dynamic> consumoMensal = new List<dynamic>();

                foreach (var litros in LitrosPorMes)
                {
                    var km = KMRodadosPorMes.Where(x => x.Mes == litros.Mes).SingleOrDefault();
                    consumoMensal.Add(new { Mes = litros.Mes, TotalKMPorLitro = km.KMRodados / litros.Total });
                }



                return consumoMensal;
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message, Inner = ex.InnerException.Message.ToString() });
            }

        }
    }
}