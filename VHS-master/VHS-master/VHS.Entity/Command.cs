using System;
using System.Collections.Generic;
using System.Text;

namespace VHS.Entity
{
    public class Command
    {
        public string Vin { get; set; }
        public bool Lights { get; set; }
        public bool Honk { get; set; }
        public bool Door { get; set; }
        public bool Heat { get; set; }
        public bool AC { get; set; }
        public bool Trunk { get; set; }
        public bool GetDest { get; set; }
        public DateTime DateLastModified { get; set; }

    }
}
