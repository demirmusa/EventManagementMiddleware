using EventManager.Shared.Dto;
using EventManager.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.EventChecker.Interfaces
{
    public interface IEventChecker
    {
        void CheckEvent<T>(EMEvent<T> eMEvent) where T : IEMEvent;
        Task CheckEventAsync<T>(EMEvent<T> eMEvent) where T : IEMEvent;
    }
}
