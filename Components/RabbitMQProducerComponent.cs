using RabbitMQ.Client;
using System.Text;

namespace ProjetoChatGPT04_CSharp.Components
{
    public class RabbitMQProducerComponent
    {
        private readonly string? _url;
        private readonly string? _user;
        private readonly string? _pass;
        private readonly int? _port;

        public RabbitMQProducerComponent(IConfiguration configuration)
        {
            _url = configuration["RabbitMQ:Url"];
            _user = configuration["RabbitMQ:User"];
            _pass = configuration["RabbitMQ:Pass"];
            _port = int.Parse(configuration["RabbitMQ:Port"]);
        }

        public void Send(string content)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = _url,
                Port = _port.Value,
                UserName = _user,
                Password = _pass
            };

            using (var connection = connectionFactory.CreateConnection())
            {
                using (var model = connection.CreateModel())
                {
                    model.QueueDeclare(
                        queue: "relatorios",
                        durable: true,
                        autoDelete: false,
                        exclusive: false,
                        arguments: null
                        );

                    model.BasicPublish(
                        exchange: string.Empty,
                        routingKey: "relatorios",
                        basicProperties: null,
                        body: Encoding.UTF8.GetBytes(content)
                        );
                }
            }
        }
    }
}



