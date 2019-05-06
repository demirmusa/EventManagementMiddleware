using EventManager.Shared.Interfaces;
namespace EventManager.Shared.Dto
{
    public class EMEvent<T> where T : IEMEvent
    {
        /// <summary>
        /// this is unique event name
        /// </summary>
        public string EventName { get; set; }
        public T EventData { get; set; }
    }
}
