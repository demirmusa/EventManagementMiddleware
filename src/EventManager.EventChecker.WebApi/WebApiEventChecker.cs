using EventManager.Shared.Dto;
using EventManager.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.EventChecker.WebApi
{
    /// <summary>
    /// this is an event checker which use web api to check event info
    /// </summary>
    public class WebApiEventChecker : IEventInfoLoader
    {
        public List<EMEventInfoDto> GetAllRegisteredEvents()
        {
            throw new NotImplementedException();
        }

        public Task<List<EMEventInfoDto>> GetAllRegisteredEventsAsync()
        {
            throw new NotImplementedException();
        }

        public EMEventInfoDto GetOrAddEMEventInfo<T>(EMEvent<T> data, string propertiesJson) where T : IEMEvent
        {
            throw new NotImplementedException();
        }

        public Task<EMEventInfoDto> GetOrAddEMEventInfoAsync<T>(EMEvent<T> data, string propertiesJson) where T : IEMEvent
        {
            throw new NotImplementedException();
        }
    }
}
