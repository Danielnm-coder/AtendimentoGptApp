using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoChatGPT04_CSharp.Dtos;
using ProjetoChatGPT04_CSharp.Services;

namespace ProjetoChatGPT04_CSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AtendimentoController : ControllerBase
    {
        private readonly AtendimentoService _atendimentoService;
        private readonly RelatorioService _relatorioService;

        public AtendimentoController(AtendimentoService atendimentoService, RelatorioService relatorioService)
        {
            _atendimentoService = atendimentoService;
            _relatorioService = relatorioService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] AtendimentoRequestDto request)
        {
            return StatusCode(200, _atendimentoService.CriarAtendimento(request));
        }

        [HttpGet("{dataMin}/{dataMax}")]
        public IActionResult Get(DateTime dataMin, DateTime dataMax)
        {
            return StatusCode(200, _relatorioService.GerarRelatorio(dataMin, dataMax));
        }
    }
}


