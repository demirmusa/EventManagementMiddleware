using EventManager.Core;
using EventManager.Shared.interfaces;

namespace NUnitTestProject1.dto
{
    class TestEvent : IEMEvent
    {
        public string TestProp { get; set; }
    }

    [EMEventInfo(eventName: "UserCreatedEvent")]
    class TestEvent2 : IEMEvent
    {
        public int userid { get; set; }
        public string userTc { get; set; }
    }
    [EMEventInfo(eventName: "UserDeletedEvent")]
    class MyEventClass : IEMEvent
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
    }
}
