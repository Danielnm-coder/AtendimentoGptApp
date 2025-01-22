using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoChatGPT04_CSharp.Dtos;
using ProjetoChatGPT04_CSharp.Services;

namespace ProjetoChatGPT04_CSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] ClienteRequestDto request)
        {
            return Ok(_clienteService.Cadastrar(request));
        }
    }
}
