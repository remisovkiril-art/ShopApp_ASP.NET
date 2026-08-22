using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopInfrastructure.Configuration;

public class RabbitMqSettings
{

    public string Host { get; set; } = null!;

    public int Port { get; set; }

}