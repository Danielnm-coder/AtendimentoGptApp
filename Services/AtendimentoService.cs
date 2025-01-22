using Newtonsoft.Json;
using ProjetoChatGPT04_CSharp.Dtos;
using ProjetoChatGPT04_CSharp.Entities;
using ProjetoChatGPT04_CSharp.Repositories;
using System.Text;
using ZstdSharp.Unsafe;

namespace ProjetoChatGPT04_CSharp.Services
{
    public class AtendimentoService
    {
        private readonly ClienteRepository _clienteRepository;
        private readonly AtendimentoRepository _atendimentoRepository;
        private readonly IConfiguration _configuration;

        public AtendimentoService(ClienteRepository clienteRepository, AtendimentoRepository atendimentoRepository, IConfiguration configuration)
        {
            _clienteRepository = clienteRepository;
            _atendimentoRepository = atendimentoRepository;
            _configuration = configuration;
        }

        public async Task<AtendimentoResponseDto> CriarAtendimento(AtendimentoRequestDto request)
        {
            // Obter os dados do cliente do atendimento
            var cliente = _clienteRepository.Find(request.EmailCliente);
            if (cliente == null)
                throw new ArgumentException("Cliente não encontrado. Faça o seu cadastro primeiro para depois gerar atendimentos.");

            // Criar a lista de mensagens que serão enviadas para o ChatGPT
            var mensagens = new List<MessageDTO>();

            // Criar o contexto inicial
            mensagens.Add(new MessageDTO
            {
                Role = "system",
                Content = $"Você é um atendente de agência de viagens especializado em fornecer informações para os clientes sobre pacotes, hotéis, vôos e passeios turísticos. O cliente que você está atendendo é o {cliente.Nome} e você sabe as seguintes informações deste cliente: {cliente.Informacoes}. Faça o atendimento de forma humanizada e cordial, tratando o cliente sempre pelo seu nome."
            });

            // Consultar os últimos atendimentos do cliente
            var atendimentos = string.Join(", ", 
                _atendimentoRepository.Find(cliente.Email)
                .Select(a => a.Conversa));

            // Adicionar histórico ao contexto
            if (!string.IsNullOrEmpty(atendimentos))
            {
                mensagens.Add(new MessageDTO
                {
                    Role = "assistant",
                    Content = atendimentos
                });
            }

            // Adicionar a pergunta do cliente
            mensagens.Add(new MessageDTO
            {
                Role = "user",
                Content = request.Texto
            });

            // Montar a requisição para a API do ChatGPT
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = mensagens
            };

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configuration["OpenAI:ApiKey"]}");
                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                var result = httpClient.PostAsync(_configuration["OpenAI:ApiUrl"], content).Result;
                var responseContent = result.Content.ReadAsStringAsync().Result;

                //verificando se ocorreu algum erro
                if (!result.IsSuccessStatusCode)
                    throw new Exception("Erro ao chamar a API do ChatGPT.");

                //deserializando e ler a resposta
                var saida = JsonConvert.DeserializeObject<ChatGPTResponseDTO>(responseContent);

                //verificando se alguma resposta foi obtida
                if (saida?.Choices != null && saida.Choices.Count > 0)
                {
                    var resposta = saida.Choices[0].Message.Content; //retornando a resposta obtida

                    // Gravar o atendimento no banco de dados
                    var atendimento = new Atendimento
                    {
                        Id = Guid.NewGuid(),
                        DataHoraAtendimento = DateTime.UtcNow,
                        EmailCliente = cliente.Email,
                        Conversa = resposta
                    };

                    _atendimentoRepository.Add(atendimento);

                    // Retornar o DTO de resposta
                    return new AtendimentoResponseDto
                    {
                        Id = atendimento.Id,
                        DataHoraAtendimento = atendimento.DataHoraAtendimento,
                        Resposta = resposta
                    };
                }
                else
                    throw new Exception("Não houve resposta para a solicitação feita.");

            }           
        }
    }
}
