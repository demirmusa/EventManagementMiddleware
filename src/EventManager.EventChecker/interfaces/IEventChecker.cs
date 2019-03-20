using FBM.Event.Shared.Dto;
using FBM.Event.Shared.interfaces;
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
        Task<List<FBMEventInfoDto>> GetAllRegisteredEventsAsync();
        List<FBMEventInfoDto> GetAllRegisteredEvents();
        Task<FBMEventInfoDto> CheckOrAddFBMEventInfo<T>(FBMEvent<T> data) where T : IFBMEvent;
        string GeneretePropertiesJson<T>(T data) where T : IFBMEvent;
    }
}
