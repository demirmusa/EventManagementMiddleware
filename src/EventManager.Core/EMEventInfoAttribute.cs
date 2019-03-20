using System;
using System.Collections.Generic;
using System.Text;

namespace EventManager.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EMEventInfoAttribute : Attribute
    {
        public string EventName { get; private set; }
        public EMEventInfoAttribute(string eventName)
        {
            EventName = eventName;
        }
    }
}
