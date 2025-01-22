using Newtonsoft.Json;
using ProjetoChatGPT04_CSharp.Components;
using ProjetoChatGPT04_CSharp.Dtos;
using ProjetoChatGPT04_CSharp.Entities;
using ProjetoChatGPT04_CSharp.Repositories;
using System.Text;

namespace ProjetoChatGPT04_CSharp.Services
{
    public class RelatorioService
    {
        private readonly AtendimentoRepository _atendimentoRepository;
        private readonly RabbitMQProducerComponent _rabbitMQProducerComponent;
        private readonly IConfiguration _configuration;

        public RelatorioService(AtendimentoRepository atendimentoRepository, RabbitMQProducerComponent rabbitMQProducerComponent, IConfiguration configuration)
        {
            _atendimentoRepository = atendimentoRepository;
            _rabbitMQProducerComponent = rabbitMQProducerComponent;
            _configuration = configuration;
        }

        public async Task<string> GerarRelatorio(DateTime dataMin, DateTime dataMax)
        {
            //consultando o histórico de atendimentos
            var atendimentos = _atendimentoRepository.FindByDatas(dataMin, dataMax);

            // Criar a lista de mensagens que serão enviadas para o ChatGPT
            var mensagens = new List<MessageDTO>();

            // Enviando o contexto inicial
            mensagens.Add(new MessageDTO
            {
                Role = "system",
                Content = $"Você é um gerador de relatórios analíticos, pegue as informações fornecidades dos históricos de atendimento de uma agência de viagens e descreva o número de atendimentos realizados, informe os destinos de viagem mais procurados e informe as dúvidas mais frequentes (passeios, hotéis etc.) e informe se houve alguma insatisfação de clientes no atendimento e se sim, e informe quais clientes ficaram insatisfeitos."
            });

            // Enviando os atendimentos
            mensagens.Add(new MessageDTO
            {
                Role = "user",
                Content = string.Join(" | ", atendimentos.Select(a => a.Conversa))
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

                    //enviando a resposta para a mensageria
                    _rabbitMQProducerComponent.Send(resposta);

                    return "Relatório gerado com sucesso. A análise será enviada para o email do gestor da empresa.";
                }
                else
                    throw new Exception("Não houve resposta para a solicitação feita.");
            }
        }
    }
}



