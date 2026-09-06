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
    private const string QueueName = "Users";

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

        _connection =
            await factory.CreateConnectionAsync(stoppingToken);

        _channel =
            await _connection.CreateChannelAsync(
                cancellationToken: stoppingToken);

        await _channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken
        );

        var consumer =
            new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();

                var json = Encoding.UTF8.GetString(body);

                var message =
                    JsonSerializer.Deserialize<UserCreateDTO>(json);

                if (message == null)
                {
                    _logger.LogWarning(
                        "RabbitMQ received an invalid user message.");

                    return;
                }

                _logger.LogInformation(
                    "RabbitMqReader received user from queue {Queue}. Email: {Email}",
                    QueueName,
                    message.Email
                );

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while processing RabbitMQ message.");
            }
        };

        await _channel.BasicConsumeAsync(
            queue: QueueName,
            autoAck: true,
            consumer: consumer,
            cancellationToken: stoppingToken
        );

        _logger.LogInformation(
            "RabbitMQ Reader started. Waiting for messages from queue: {Queue}",
            QueueName
        );

        try
        {
            await Task.Delay(
                Timeout.Infinite,
                stoppingToken);
        }
        catch (OperationCanceledException)
        {
        }
    }

    public override async Task StopAsync(
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "RabbitMQ Reader stopping...");

        if (_channel != null)
        {
            await _channel.CloseAsync(
                cancellationToken);
        }

        if (_connection != null)
        {
            await _connection.CloseAsync(
                cancellationToken);
        }

        await base.StopAsync(cancellationToken);
    }
}