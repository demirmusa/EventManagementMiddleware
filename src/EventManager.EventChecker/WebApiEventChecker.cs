using EventManager.EventChecker.interfaces;
using EventManager.Shared.Dto;
using EventManager.Shared.interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.EventChecker
{
    /// <summary>
    /// this is an event checker which use web api to check event info
    /// </summary>
    public class WebApiEventChecker : IEventChecker
    {
        public Task<EMEventInfoDto> CheckOrAddEMEventInfo<T>(EMEvent<T> data) where T : IEMEvent
        {
            throw new NotImplementedException();
        }

        public string GeneretePropertiesJson<T>(T data) where T : IEMEvent
        {
            throw new NotImplementedException();
        }

     

        public Task<List<EMEventInfoDto>> GetAllRegisteredEventsAsync()
        {
            throw new NotImplementedException();
        }

        public List<EMEventInfoDto> GetAllRegisteredEvents()
        {
            throw new NotImplementedException();
        }
    }
}
