using HiQ.NetStandard.Util.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VHS.Entity;

namespace VHSBackend.Core.Repository
{
    public class SqlGPSRepository : ADbRepositoryBase
    {
        Status status = new Status();

        public bool DeleteDestinationFromDB(string vin)
        {
            var parameters = new SqlParameters();

            parameters.AddNVarChar("@vin", 50, vin);
            DbAccess.ExecuteNonQuery("dbo.sDeleteDestination", ref parameters, System.Data.CommandType.StoredProcedure);

            return true;
        }

        public double getLatitudeFromDB(string vin)
        {
            var parameters = new SqlParameters();
            parameters.AddNVarChar("@vin", 50, vin);
            parameters.AddBoolean("@result", false, ParameterDirection.Output);
            parameters.AddFloat("@gps_latitude", 0.0, ParameterDirection.Output);

            DbAccess.ExecuteNonQuery("dbo.sGetLatitude", ref parameters, CommandType.StoredProcedure);

            if (parameters.GetBool("@result"))
            {
                return status.Gps_Latitude = parameters.GetDouble("@gps_latitude");
            }

            return 0.0;
        }

        public double getLongitudeFromDB(string vin)
        {
            var parameters = new SqlParameters();
            parameters.AddNVarChar("@vin", 50, vin);
            parameters.AddBoolean("@result", false, ParameterDirection.Output);
            parameters.AddFloat("@gps_longitude", 0.0, ParameterDirection.Output);

            DbAccess.ExecuteNonQuery("dbo.sGetLongitude", ref parameters, CommandType.StoredProcedure);

            if (parameters.GetBool("@result"))
            {
                return status.Gps_Latitude = parameters.GetDouble("@gps_longitude");
            }

            return 0.0;
        }

        // add first destination
        public void logDestinationInRouteTable(string vin, double longitude, double latitude)
        {
            var parameters = new SqlParameters();
            parameters.AddNVarChar("@vin", 50, vin);
            parameters.AddFloat("@latitude", latitude, ParameterDirection.Input);
            parameters.AddFloat("@longitude", longitude, ParameterDirection.Input);
            parameters.AddDateTime("@timestamp", DateTime.Now, ParameterDirection.Input);

            DbAccess.ExecuteNonQuery("dbo.sAddDestination", ref parameters, CommandType.StoredProcedure);
        }

        // update destination
        public void updateDestinationInRouteTable(string vin, double longitude, double latitude)
        {
            var parameters = new SqlParameters();
            parameters.AddNVarChar("@vin", 50, vin);
            parameters.AddFloat("@latitude", latitude, ParameterDirection.Input);
            parameters.AddFloat("@longitude", longitude, ParameterDirection.Input);

            DbAccess.ExecuteNonQuery("dbo.sUpdateDestination", ref parameters, CommandType.StoredProcedure);
        }
    }
}
