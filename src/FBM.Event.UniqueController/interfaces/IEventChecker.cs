using FBM.Event.UniqueController.Data.dbEntities;
using FBM.Event.UniqueController.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FBM.Event.UniqueController.interfaces
{
    public interface IEventChecker
    {
        Task<List<FBMEventInfo>> GetAllRegisteredEvents();
        Task<FBMEventInfo> CheckOrAddFBMEventInfo<T>(FBMEventInfoRequestDto<T> data) where T : new();
        string GeneretePropertiesJson<T>(T data) where T : new();
    }
}
