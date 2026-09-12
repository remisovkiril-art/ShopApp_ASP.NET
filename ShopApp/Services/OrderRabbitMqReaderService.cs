using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ShopApplication.DTOs.OrderDTOs;
using ShopApplication.Interfaces.Services;
using ShopInfrastructure.Configuration;
using ShopInfrastructure.Data;
using System.Text;
using System.Text.Json;

namespace ShopApi.Services;

public class OrderRabbitMqReaderService : BackgroundService
{
    private const string QueueName = "Orders";

    private readonly ILogger<OrderRabbitMqReaderService> _logger;
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly IServiceScopeFactory _scopeFactory;

    private IConnection? _connection;
    private IChannel? _channel;

    public OrderRabbitMqReaderService(
        ILogger<OrderRabbitMqReaderService> logger,
        IOptions<RabbitMqSettings> options,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _rabbitMqSettings = options.Value;
        _scopeFactory = scopeFactory;
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
                    JsonSerializer.Deserialize<OrderCreateDTO>(json);

                if (message == null)
                {
                    _logger.LogWarning(
                        "RabbitMQ received an invalid order message.");

                    return;
                }

                await ProcessOrderAsync(
                    message,
                    stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while processing order from RabbitMQ.");
            }
        };

        await _channel.BasicConsumeAsync(
            queue: QueueName,
            autoAck: true,
            consumer: consumer,
            cancellationToken: stoppingToken
        );

        _logger.LogInformation(
            "Order RabbitMQ Reader started. Waiting for messages from queue: {Queue}",
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

    private async Task ProcessOrderAsync(
        OrderCreateDTO dto,
        CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var context =
            scope.ServiceProvider.GetRequiredService<ShopDbContext>();

        var emailService =
            scope.ServiceProvider.GetRequiredService<IEmailService>();

        var user = await context.Users
            .FirstOrDefaultAsync(
                user =>
                    user.Id == dto.UserId &&
                    user.IsActive,
                cancellationToken);

        if (user == null)
        {
            _logger.LogWarning(
                "Order rejected. User {UserId} was not found.",
                dto.UserId);

            return;
        }

        if (dto.OrderDetails == null ||
            dto.OrderDetails.Count == 0)
        {
            await emailService.SendOrderEmailAsync(
                user.Email,
                "Замовлення не може бути створене, тому що список товарів порожній.");

            return;
        }

        var productIds = dto.OrderDetails
            .Select(detail => detail.ProductId)
            .Distinct()
            .ToList();

        var products = await context.Products
            .Where(product =>
                productIds.Contains(product.Id) &&
                product.IsActive)
            .ToListAsync(cancellationToken);

        foreach (var detail in dto.OrderDetails)
        {
            if (detail.Count <= 0)
            {
                await emailService.SendOrderEmailAsync(
                    user.Email,
                    $"Замовлення не може бути створене. Некоректна кількість товару {detail.ProductId}.");

                return;
            }

            var product = products
                .FirstOrDefault(
                    product => product.Id == detail.ProductId);

            if (product == null)
            {
                await emailService.SendOrderEmailAsync(
                    user.Email,
                    $"Товар з ID {detail.ProductId} відсутній на складі. Замовлення очікує.");

                return;
            }

            if (product.StockQty < detail.Count)
            {
                await emailService.SendOrderEmailAsync(
                    user.Email,
                    $"Товар \"{product.Name}\" відсутній у необхідній кількості. " +
                    $"Потрібно: {detail.Count}, доступно: {product.StockQty}. " +
                    $"Замовлення очікує.");

                return;
            }
        }

        var order = new ShopDomain.Models.Order
        {
            UserId = dto.UserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Status = dto.Status,
            Paid = dto.Paid
        };

        decimal totalPrice = 0;

        foreach (var detail in dto.OrderDetails)
        {
            var product = products
                .First(product => product.Id == detail.ProductId);

            var orderDetail = new ShopDomain.Models.OrderDetail
            {
                ProductId = product.Id,
                Price = product.Price,
                Count = detail.Count
            };

            order.OrderDetails.Add(orderDetail);

            product.StockQty -= detail.Count;

            totalPrice += product.Price * detail.Count;
        }

        context.Orders.Add(order);

        await context.SaveChangesAsync(
            cancellationToken);

        var emailText = new StringBuilder();

        emailText.AppendLine("Ваше замовлення створено.");
        emailText.AppendLine();
        emailText.AppendLine("Товари:");

        foreach (var detail in order.OrderDetails)
        {
            var product = products
                .First(product => product.Id == detail.ProductId);

            emailText.AppendLine(
                $"{product.Name} - " +
                $"ціна: {detail.Price} грн, " +
                $"кількість: {detail.Count}");
        }

        emailText.AppendLine();
        emailText.AppendLine(
            $"Загальна ціна: {totalPrice} грн");

        await emailService.SendOrderEmailAsync(
            user.Email,
            emailText.ToString());

        _logger.LogInformation(
            "Order for user {UserId} was created successfully.",
            dto.UserId);
    }

    public override async Task StopAsync(
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Order RabbitMQ Reader stopping...");

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