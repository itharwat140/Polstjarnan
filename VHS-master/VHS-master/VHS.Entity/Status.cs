using System;
using System.Collections.Generic;
using System.Text;

namespace VHS.Entity
{
    public class Status
    {
        public string Vin { get; set; }
        public int Battery { get; set; }
        public bool Lock { get; set; }
        public double Gps_Longitude { get; set; }
        public double Gps_Latitude { get; set; }
        public bool Alarm { get; set; }
        public string TirePressure { get; set; }
        public double Milage { get; set; }
        public DateTime DateLastModified { get; set; }


    }
}
