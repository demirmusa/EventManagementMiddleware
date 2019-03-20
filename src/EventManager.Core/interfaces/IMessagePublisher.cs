using System;
using System.Collections.Generic;
using System.Text;

namespace EventManager.Core.interfaces
{
    public interface IMessagePublisher
    {
        void Publish(string message);
    }
}
