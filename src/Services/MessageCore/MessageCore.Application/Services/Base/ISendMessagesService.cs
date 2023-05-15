using MessageCore.Domain.AggregatesModels.SendingOrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Application.Services.Base
{
    public interface ISendMessagesService
    {
        Task SendAsync(SendingOrder sendingOrder);
    }
}

