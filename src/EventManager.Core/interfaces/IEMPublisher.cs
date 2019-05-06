using EventManager.Shared.Interfaces;
using System.Threading.Tasks;

namespace EventManager.Core.Interfaces
{
    public interface IEMPublisher
    {
        Task PublishAsync<T>(T nodeEvent) where T : IEMEvent;
        void Publish<T>(T nodeEvent) where T : IEMEvent;
    }
}
