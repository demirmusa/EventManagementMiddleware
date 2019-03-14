using System;
using System.Collections.Generic;
using System.Text;

namespace FBM.Event.Client
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FBMEventInfoAttribute : Attribute
    {
        public string EventName { get; private set; }
        public FBMEventInfoAttribute(string eventName)
        {
            EventName = eventName;
        }
    }
}
