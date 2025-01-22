using ProjetoChatGPT04_CSharp.Components;
using ProjetoChatGPT04_CSharp.Repositories;
using ProjetoChatGPT04_CSharp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region configurar as injeções de dependência

builder.Services.AddTransient<ClienteRepository>();
builder.Services.AddTransient<AtendimentoRepository>();
builder.Services.AddTransient<ClienteService>();
builder.Services.AddTransient<AtendimentoService>();
builder.Services.AddTransient<RelatorioService>();
builder.Services.AddTransient<RabbitMQProducerComponent>();

builder.Services.AddHostedService<RabbitMQConsumerComponent>();

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
