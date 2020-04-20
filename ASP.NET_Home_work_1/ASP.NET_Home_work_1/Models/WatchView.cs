using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Home_work_1.Models
{
    public class WatchView
    {
        public int Id { get; set; }
        public string LabelName { get; set; }
        public string TypeOfMechanism { get; set; }
        public string Price { get; set; }
        public string WaterResist { get; set; }
        public string Sex { get; set; }
        public string CaseMaterial { get; set; }
        public int NumberOfHands { get; set; }
        public int BeatPerHour { get; set; }
    }
}
