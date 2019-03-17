using FBM.Event.Shared.Dto;
using FBM.Event.Shared.interfaces;
using FBM.Event.UniqueController.Data.dbEntities;
using FBM.Event.UniqueController.Dto;
using FBM.Event.UniqueController.interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FBM.Event.UniqueController
{
    /// <summary>
    /// this is an event checker which use web api to check event info
    /// </summary>
    public class WebApiEventChecker : IEventChecker
    {
        public Task<FBMEventInfoDto> CheckOrAddFBMEventInfo<T>(FBMEvent<T> data) where T : IFBMEvent
        {
            throw new NotImplementedException();
        }

        public string GeneretePropertiesJson<T>(T data) where T : IFBMEvent
        {
            throw new NotImplementedException();
        }

     

        public Task<List<FBMEventInfoDto>> GetAllRegisteredEventsAsync()
        {
            throw new NotImplementedException();
        }

        public List<FBMEventInfoDto> GetAllRegisteredEvents()
        {
            throw new NotImplementedException();
        }
    }
}
