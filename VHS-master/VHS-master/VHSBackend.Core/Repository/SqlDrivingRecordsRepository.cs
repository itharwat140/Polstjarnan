using HiQ.NetStandard.Util.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using VHS.Entity;

namespace VHSBackend.Core.Repository
{
    public class SqlDrivingRecordsRepository : ADbRepositoryBase
    {
        public Guid StartDrivingJournal(string vin)
        {
            Guid journal_id = Guid.NewGuid();
            var parameters = new SqlParameters();
            parameters.AddNVarChar("@vin", vin, 50, ParameterDirection.Input);
            parameters.AddUniqueIdentifier("@journal_id", journal_id, ParameterDirection.Input);
            parameters.AddDateTime("@startTime", DateTime.Now, ParameterDirection.Input);
            parameters.AddDateTime("@endTime", null, ParameterDirection.Input);
            parameters.AddInt("@tripStatus", 0, ParameterDirection.Input);
            parameters.AddInt("@tripDistance", 0, ParameterDirection.Input);
            parameters.AddInt("@tripEnergyConsumption", 0, ParameterDirection.Input);
            parameters.AddInt("@tripAverageEnergyCons", 0, ParameterDirection.Input);
            parameters.AddInt("@tripAverageSpeed", 0, ParameterDirection.Input);
            parameters.AddDateTime("@tripDate", null, ParameterDirection.Input);

            DbAccess.ExecuteNonQuery("dbo.sStartDrivingJournal", ref parameters, CommandType.StoredProcedure);
            return journal_id;
        }

        public void sendDrivingLogs(string vin, Guid journal_id, DriveLogData logData)
        {
            var parameters = new SqlParameters();
            parameters.AddNVarChar("@vin", vin, 50, ParameterDirection.Input);
            parameters.AddUniqueIdentifier("@journal_id", journal_id, ParameterDirection.Input);
            parameters.AddFloat("@longitude", logData.Longitude, ParameterDirection.Input);
            parameters.AddFloat("@latitude", logData.Latitude, ParameterDirection.Input);
            parameters.AddInt("@battery_level", logData.BatteryLevel, ParameterDirection.Input);
            parameters.AddInt("@current_milage", logData.CurrentMilage, ParameterDirection.Input);
            parameters.AddDateTime("@created_at", DateTime.Now, ParameterDirection.Input);

            DbAccess.ExecuteNonQuery("dbo.sSendDrivingLogs", ref parameters, CommandType.StoredProcedure);
        }

        public IList<DriveLogData> GetTripLogs(string vin, Guid journal_id)
        {

            var result = new List<DriveLogData>();
            var parameters = new SqlParameters();
            parameters.AddNVarChar("@vin", vin, 50, ParameterDirection.Input);
            parameters.AddUniqueIdentifier("@journal_id", journal_id, ParameterDirection.Input);

            var dr = DbAccess.ExecuteReader("dbo.sGetTripLogs", ref parameters, CommandType.StoredProcedure);
            while (dr.Read())
            {
                var l = new DriveLogData();
                l.Longitude = dr.GetDouble(1);
                l.Latitude = dr.GetDouble(2);
                l.BatteryLevel = dr.GetInt32(3);
                l.CurrentMilage = dr.GetInt32(4);
                l.CreatedAt = dr.GetDateTime(5);
                result.Add(l);
            }

            DbAccess.DisposeReader(ref dr);

            var startMilage = result.First().CurrentMilage;
            var endMilage = result.Last().CurrentMilage;
            var startBatteryLevel = result.First().BatteryLevel;
            var endBatteryLevel = result.Last().BatteryLevel;
            DateTime startTime = result.First().CreatedAt;
            DateTime endTime = result.Last().CreatedAt;
            System.TimeSpan tripTime = endTime.Subtract(startTime);

            DrivingTripCalculations(vin, journal_id, startMilage, endMilage, startBatteryLevel, endBatteryLevel, tripTime.TotalSeconds, endTime);

            return result;
        }

        public TripData DrivingTripCalculations(string vin, Guid journal_id, int startMilage, int endMilage, int startBatteryLevel,  int endBatteryLevel, double tripTime, DateTime endTime)
        {
            TripData tripData = new TripData();
            tripData.Journal_id = journal_id;
            tripData.TripDistance = endMilage - startMilage;
            tripData.TripEnergyCosumption = startBatteryLevel - endBatteryLevel;
            tripData.TripAvrEnergyCons = ((tripData.TripDistance) / (tripData.TripEnergyCosumption));
            tripData.EndTime = endTime;
            tripData.TripAvrSpeed = (int)(tripData.TripDistance / (double)(tripTime / 3600));

            SaveTripDataInDB(vin, tripData);

            return tripData;
        }

        public TripData SaveTripDataInDB(string vin, TripData tripData)
        {
            var parameters = new SqlParameters();
            parameters.AddNVarChar("@vin", vin, 50, ParameterDirection.Input);
            parameters.AddUniqueIdentifier("@journal_id", tripData.Journal_id, ParameterDirection.Input);
            parameters.AddDateTime("@startTime", DateTime.Now, ParameterDirection.Input);
            parameters.AddDateTime("@endTime", tripData.EndTime, ParameterDirection.Input);
            parameters.AddInt("@tripStatus", 1, ParameterDirection.Input);
            parameters.AddInt("@tripDistance", tripData.TripDistance, ParameterDirection.Input);
            parameters.AddInt("@tripEnergyConsumption", tripData.TripEnergyCosumption, ParameterDirection.Input);
            parameters.AddInt("@tripAverageEnergyCons", tripData.TripAvrEnergyCons, ParameterDirection.Input);
            parameters.AddInt("@tripAverageSpeed", tripData.TripAvrSpeed, ParameterDirection.Input);
            parameters.AddDateTime("@tripDate", DateTime.Today, ParameterDirection.Input);

            DbAccess.ExecuteNonQuery("dbo.sSaveTripData", ref parameters, CommandType.StoredProcedure);

            return tripData;
        }
    }
}
