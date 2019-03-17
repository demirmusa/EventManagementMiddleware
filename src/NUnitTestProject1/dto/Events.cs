using FBM.Event.Client;
using FBM.Event.Shared.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NUnitTestProject1.dto
{
    class TestEvent : IFBMEvent
    {
        public string TestProp { get; set; }
    }

    [FBMEventInfo(eventName: "UserCreatedEvent")]
    class TestEvent2 : IFBMEvent
    {
        public int userid { get; set; }
        public string userTc { get; set; }
    }
    [FBMEventInfo(eventName: "UserDeletedEvent")]
    class MyEventClass : IFBMEvent
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
    }
}
