using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Common.Model
{
    public class VpicResponse
    {
        public int Count { get; set; }
        public string Message { get; set; }
        public List<Vehicle> Results { get; set; }
        public string SearchCriteria { get; set; }
    }
}
