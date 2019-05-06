using EventManager.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Shared.Interfaces
{
    public interface IEventInfoLoader
    {
        Task<List<EMEventInfoDto>> GetAllRegisteredEventsAsync();
        Task<EMEventInfoDto> GetOrAddEMEventInfoAsync<T>(EMEvent<T> data, string propertiesJson) where T : IEMEvent;

        List<EMEventInfoDto> GetAllRegisteredEvents();
        EMEventInfoDto GetOrAddEMEventInfo<T>(EMEvent<T> data, string propertiesJson) where T : IEMEvent;
    }
}
