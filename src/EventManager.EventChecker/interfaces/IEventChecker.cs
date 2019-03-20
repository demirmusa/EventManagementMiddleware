
using EventManager.Shared.Dto;
using EventManager.Shared.interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManager.EventChecker.interfaces
{
    public interface IEventChecker
    {
        Task<List<EMEventInfoDto>> GetAllRegisteredEventsAsync();
        Task<EMEventInfoDto> CheckOrAddEMEventInfoAsync<T>(EMEvent<T> data) where T : IEMEvent;

        List<EMEventInfoDto> GetAllRegisteredEvents();
        EMEventInfoDto CheckOrAddEMEventInfo<T>(EMEvent<T> data) where T : IEMEvent;

        string GeneretePropertiesJson<T>(T data) where T : IEMEvent;
    }
}
