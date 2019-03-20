using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Core.interfaces
{
    public interface IMessagePublisher
    {
        void Publish(string message);
        Task PublishAsync(string message);
    }
}
