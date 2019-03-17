using FBM.Event.Shared.interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FBM.Event.Client.interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T nodeEvent) where T : IFBMEvent;
    }
}
