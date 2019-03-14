using System;
using System.Collections.Generic;
using System.Text;

namespace FBM.Event.UniqueController.Dto
{
    public class FBMEventInfoRequestDto<T>
    {
        public string EventName { get; set; }
        public T Event { get; set; }
    }
}
