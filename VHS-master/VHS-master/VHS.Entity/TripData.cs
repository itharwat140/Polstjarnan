using System;
using System.Collections.Generic;
using System.Text;

namespace VHS.Entity
{
    public class TripData
    {
        public string Vin { get; set; }
        public Guid Journal_id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TripStatus { get; set; }
        public int TripDistance { get; set; }
        public int TripEnergyCosumption { get; set; }
        public int TripAvrEnergyCons { get; set; }
        public int TripAvrSpeed { get; set; }
        public DateTime TripDate { get; set; }
    }
}
