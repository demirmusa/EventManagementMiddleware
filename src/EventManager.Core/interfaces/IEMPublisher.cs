using EventManager.Shared.interfaces;
using System.Threading.Tasks;

namespace EventManager.Core.interfaces
{
    public interface IEMPublisher
    {
        Task PublishAsync<T>(T nodeEvent) where T : IEMEvent;
    }
}
