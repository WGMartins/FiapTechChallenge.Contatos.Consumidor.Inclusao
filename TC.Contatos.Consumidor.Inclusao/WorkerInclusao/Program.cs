using Domain.Interfaces;
using FluentValidation;
using Infrastructure.RabbitMQ;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using UseCase.ContatoUseCase.Adicionar;
using UseCase.Interfaces;
using WorkerInclusao;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("ConnectionString"));
}, ServiceLifetime.Scoped);

builder.Services.AddScoped<IContatoRepository, ContatoRepository>();

builder.Services.AddScoped<IAdicionarContatoUseCase, AdicionarContatoUseCase>();
builder.Services.AddScoped<IValidator<AdicionarContatoDto>, AdicionarContatoValidator>();

//RabbitMQ
builder.Services.AddSingleton<IMessageConsumer, RabbitMQMessageConsumer>();
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddSingleton<ConnectionFactory>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value;
    return new ConnectionFactory
    {
        HostName = settings.HostName,
        UserName = settings.UserName,
        Password = settings.Password,
    };
});

builder.Services.AddSingleton<Func<Task<IConnection>>>(sp =>
{
    var factory = sp.GetRequiredService<ConnectionFactory>();
    return () => factory.CreateConnectionAsync();
});
//RabbitMQ

var host = builder.Build();
host.Run();
