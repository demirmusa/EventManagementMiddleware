using EventManager.Shared.Dto;
using EventManager.Shared.Interfaces;
using System.Threading.Tasks;

namespace EventManager.Core.Interfaces
{
    public interface IEventManagerCore
    {
        //EMEvent<T> GetEvent<T>(T e) where T : IEMEvent;
        Task<EMEvent<T>> GetEventAsync<T>(T e) where T : IEMEvent;
        EMEvent<T> GetEvent<T>(T e) where T : IEMEvent;
    }
}
