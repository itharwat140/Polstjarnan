using System;
using System.Collections.Generic;
using System.Text;

namespace VHS.Entity
{
    public class DriveLogData
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int BatteryLevel { get; set; }
        public int CurrentMilage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
