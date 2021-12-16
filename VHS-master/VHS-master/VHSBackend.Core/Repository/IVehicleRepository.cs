using System;
using System.Collections.Generic;
using System.Text;
using VHS.Entity;

namespace VHSBackend.Core.Repository
{
    public interface IVehicleRepository
    {
        string SearchVehicle(string vin);
    }
}
