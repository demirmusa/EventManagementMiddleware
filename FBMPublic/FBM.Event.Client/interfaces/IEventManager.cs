using FBM.Event.Client.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBM.Event.Client.interfaces
{
    public interface IEventManager
    {
        FBMEvent<T> GetEvent<T>(T e) where T : IFBMEvent;
    }
}
