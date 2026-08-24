namespace ShopInfrastructure.Configuration;

public sealed class RabbitMqSettings
{
    public string Host { get; set; } = null!;
    public int Port { get; set; }
}