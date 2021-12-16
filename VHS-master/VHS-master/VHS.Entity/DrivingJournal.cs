using System;
using System.Collections.Generic;
using System.Text;

namespace VHS.Entity
{
    public class DrivingJournal
    {
        public string Vin { get; set; }
        public Guid JournalID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TripStatus { get; set; }
        public int TripDistance { get; set; }
        public int TripEnergyConsumption { get; set; }
        public int TripAverageEnergyCons { get; set; }
        public int TripAverageSpeed { get; set; }
        public DateTime TripDate { get; set; }

    }
}
