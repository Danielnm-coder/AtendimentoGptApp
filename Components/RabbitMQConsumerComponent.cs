

using MongoDB.Bson;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Mail;
using System.Text;


namespace ProjetoChatGPT04_CSharp.Components
{
    public class RabbitMQConsumerComponent : BackgroundService
    {
        private readonly string? _url;
        private readonly string? _user;
        private readonly string? _pass;
        private readonly int? _port;


        public RabbitMQConsumerComponent(IConfiguration configuration)
        {
            _url = configuration["RabbitMQ:Url"];
            _user = configuration["RabbitMQ:User"];
            _pass = configuration["RabbitMQ:Pass"];
            _port = int.Parse(configuration["RabbitMQ:Port"]);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = _url,
                Port = _port.Value,
                UserName = _user,
                Password = _pass
            };


            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();


            model.QueueDeclare(
                queue: "relatorios",
                durable: true,
                autoDelete: false,
                exclusive: false,
                arguments: null
                );


            var consumer = new EventingBasicConsumer(model);


            consumer.Received += (sender, args) =>
            {
                var payload = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(payload);


                string smtpHost = "localhost";
                int smtpPort = 1025;
                string fromEmail = "noreply@example.com";
                string toEmail = "user@example.com";
                string subject = "Relatório de atendimentos gerado em: " + DateTime.Now;
                string body = message;


                var smtpClient = new SmtpClient(smtpHost, smtpPort)
                {
                    EnableSsl = false
                };


                var mailMessage = new MailMessage(fromEmail, toEmail, subject, body);


                smtpClient.Send(mailMessage);


                model.BasicAck(args.DeliveryTag, false);
            };


            model.BasicConsume(
                queue: "relatorios",
                autoAck: false,
                consumer: consumer
                );
        }
    }
}





