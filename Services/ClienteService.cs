using ProjetoChatGPT04_CSharp.Dtos;
using ProjetoChatGPT04_CSharp.Entities;
using ProjetoChatGPT04_CSharp.Repositories;

namespace ProjetoChatGPT04_CSharp.Services
{
    public class ClienteService
    {
        private readonly ClienteRepository _clienteRepository;

        public ClienteService(ClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public ClienteResponseDto Cadastrar(ClienteRequestDto request)
        {
            var cliente = new Cliente
            {
                Id = Guid.NewGuid(),
                Nome = request.Nome,
                Email = request.Email,
                Informacoes = request.Informacoes,
            };

            _clienteRepository.Add(cliente);

            var response = new ClienteResponseDto
            {
                Id = cliente.Id,
                DataHoraCadastro = DateTime.Now.ToString(),
                Message = "Cadastro realizado com sucesso!"
            };

            return response;
        }
    }
}
