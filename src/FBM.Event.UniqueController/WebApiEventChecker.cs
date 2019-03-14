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
        public Task<FBMEventInfo> CheckOrAddFBMEventInfo<T>(FBMEventInfoRequestDto<T> data) where T : new()
        {
            throw new NotImplementedException();
        }

        public string GeneretePropertiesJson<T>(T data) where T : new()
        {
            throw new NotImplementedException();
        }

        public Task<List<FBMEventInfo>> GetAllRegisteredEvents()
        {
            throw new NotImplementedException();
        }
    }
}
