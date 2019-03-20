using FBM.Event.Shared.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBM.Event.Shared.Dto
{
    public class FBMEvent<T> where T : IFBMEvent
    {
        /// <summary>
        /// this is unieuq event name
        /// </summary>
        public string EventName { get; set; }
        public T EventData { get; set; }
    }
}
