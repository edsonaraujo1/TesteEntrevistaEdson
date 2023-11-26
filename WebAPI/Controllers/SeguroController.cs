using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [EnableCors("CorsPolicy")]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SeguroController : Controller
    {
        private readonly ISeguroRepository _seguroRepositories;
        private readonly IMapper _mapper;
        public SeguroController(ISeguroRepository seguroRepositories, IMapper mapper)
        {
            _mapper = mapper;
            _seguroRepositories = seguroRepositories;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seguro>>> GetSeguros()
        {
            return Ok(await _seguroRepositories.SelecionarTodos());
        }
        [HttpPost]
        public async Task<ActionResult> CadastrarSeguro(Seguro seguro)
        {
            CalculaValorSeguro calculo = new CalculaValorSeguro();
            double com_pre = calculo.CalculaSeguro(seguro.ValorVeiculo);
            seguro.ValorSeguro = com_pre;

            _seguroRepositories.Incluir(seguro);
            if (await _seguroRepositories.SaveAllAsync())
            {
                return Ok("Registro Salvo com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao salvar registro.");
        }
        [HttpPut]
        public async Task<ActionResult> AlterarSeguro(Seguro seguro)
        {
            CalculaValorSeguro calculo = new CalculaValorSeguro();
            double com_pre = calculo.CalculaSeguro(seguro.ValorVeiculo);
            seguro.ValorSeguro = com_pre;

            _seguroRepositories.Alterar(seguro);
            if (await _seguroRepositories.SaveAllAsync())
            {
                return Ok("Registro Alterado com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao alterar registro.");
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> ExcluirSeguro(int id)
        {
            var seguro = await _seguroRepositories.SelecionarByPk(id);
            if (seguro == null)
            {
                return NotFound("Registro não encontrdo!");
            }
            _seguroRepositories.Excluir(seguro);
            if (await _seguroRepositories.SaveAllAsync())
            {
                return Ok("Registro Excluido com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao excluir o registro.");
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> SelecionarSeguro(int id)
        {
            var seguro = await _seguroRepositories.SelecionarByPk(id);
            if (seguro == null)
            {
                return NotFound("Registro não encontrdo!");
            }
            //SeguroDTO seguroDTO = new SeguroDTO
            //{
            //    Cliente = seguro.Cliente,
            //    Marca = seguro.Marca,
            //    Veiculo = seguro.Veiculo,
            //    Modelo = seguro.Modelo,
            //    ValorSeguro = seguro.ValorSeguro,
            //    ValorVeiculo = seguro.ValorVeiculo
            //};
            //var seguroDTO = _mapper.Map<SeguroDTO>(seguro);
            return Ok(seguro);
        }

        public class CalculaValorSeguro
        {
            private const double margem = 0.03;
            private const double recebe = 0.05;

            public double CalculaSeguro(double VlVeiculo)
            {
                double risk_01 = (VlVeiculo * 5) / (2 * VlVeiculo);
                double risk_02 = (risk_01 / 100) * VlVeiculo;
                double Premio = risk_02 * (1 + margem);
                double valorfinal = recebe * Premio;

                return valorfinal;
            }
        }
    }
}
