using FBM.Event.Client.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBM.Event.Client.Dto
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
