using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApplication.Interfaces.Services;

public interface IQueueService
{
    Task PublishAsync<T>(string queue, T message);

}