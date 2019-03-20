using System;
using System.Collections.Generic;
using System.Text;

namespace FBM.Event.UniqueController.Dto
{
    public class WebApiOptions
    {
        public string BaseUrl { get; set; }
        public string GetAllEventsEndPoint { get; set; }
        public string CheckOrAddFBMEventInfoEndPoint { get; set; }
        public string GeneretePropertiesJsonEndPoint { get; set; }

        ///.... this is sample. i will change all wep api structure
    }
}
