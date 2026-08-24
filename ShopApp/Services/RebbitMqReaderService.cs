using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ShopApplication.DTOs.UserDTOs;
using ShopInfrastructure.Configuration;
using System.Text;
using System.Text.Json;

namespace ShopApi.Services;

public class RabbitMqReaderService : BackgroundService
{
    private readonly ILogger<RabbitMqReaderService> _logger;
    private readonly RabbitMqSettings _rabbitMqSettings;

    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqReaderService(
        ILogger<RabbitMqReaderService> logger,
        IOptions<RabbitMqSettings> options)
    {
        _logger = logger;
        _rabbitMqSettings = options.Value;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqSettings.Host,
            Port = _rabbitMqSettings.Port
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(
            queue: "Users",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (sender, e) =>
        {
            var body = e.Body.ToArray();

            var json = Encoding.UTF8.GetString(body);

            var message = JsonSerializer.Deserialize<UserCreateDTO>(json);

            if (message == null)
                return;

            _logger.LogInformation(
                "RabbitMqReader Received - Email: {Email}",
                message.Email
            );

            _logger.LogInformation(
                "RabbitMqReader Received - Password: {Password}",
                message.Password
            );

            await Task.CompletedTask;
        };

        await _channel.BasicConsumeAsync(
            queue: "Users",
            autoAck: true,
            consumer: consumer
        );

        _logger.LogInformation(
            "RabbitMQ Reader started. Waiting messages..."
        );

        await Task.Delay(
            Timeout.Infinite,
            stoppingToken
        );
    }

    public override async Task StopAsync(
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "RabbitMQ Reader stopping..."
        );

        if (_channel != null)
            await _channel.CloseAsync();

        if (_connection != null)
            await _connection.CloseAsync();

        await base.StopAsync(cancellationToken);
    }
}