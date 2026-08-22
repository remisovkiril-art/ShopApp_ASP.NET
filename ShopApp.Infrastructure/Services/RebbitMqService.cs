using Microsoft.Extensions.Options;
using ShopApplication.Interfaces.Services;
using ShopInfrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;
namespace ShopInfrastructure.Services;

public class RabbitMqService : IQueueService
{

    private readonly RabbitMqSettings _rabbitMqSettings;


    public RabbitMqService(IOptions<RabbitMqSettings> options)
    {
        _rabbitMqSettings = options.Value;
    }
    public async Task PublishAsync<T>(string queue, T message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMqSettings.Host,
            Port = _rabbitMqSettings.Port
        };
        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
        await channel.QueueDeclareAsync(
            queue: queue,         
            durable: true,         
            exclusive: false,     
            autoDelete: false,    
            arguments: null        
        );
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);
        var properties = new BasicProperties
        {
            Persistent = true
        };
        await channel.BasicPublishAsync(
             exchange: "",        
             routingKey: queue,  
             mandatory: false,    
             basicProperties: properties,
             body: body         
        );
    }
}