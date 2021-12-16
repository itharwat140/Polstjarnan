using HiQ.NetStandard.Util.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using VHS.Entity;
using VHSBackend.Core.Integrations;

namespace VHSBackend.Core.Repository
{
    public class SqlVehicleRepository : ADbRepositoryBase, IVehicleRepository
    {
        public SqlVehicleRepository()
        {
            _cdsClient = new CdsClient();
        }
        private readonly CdsClient _cdsClient;
        

        public Status GetStatus(string vin)
        {
            var parameters = new SqlParameters();
            parameters.AddNVarChar("@vin", 50, vin);
            parameters.AddBoolean("@result", false, ParameterDirection.Output);
            parameters.AddBoolean("@lock", false, ParameterDirection.Output);
            parameters.AddInt("@battery", 0, ParameterDirection.Output);
            parameters.AddFloat("@gps_longitude", 0.0, ParameterDirection.Output);
            parameters.AddFloat("@gps_latitude", 0.0, ParameterDirection.Output);
            parameters.AddBoolean("@alarm", false, ParameterDirection.Output);
            parameters.AddNVarChar("@tirepressure", 50,  "0, 0, 0, 0 ", ParameterDirection.Output);
            parameters.AddFloat("@milage", 0.0, ParameterDirection.Output);
            parameters.AddDateTime("@DateLastModified", DateTime.Now, ParameterDirection.Output);

            DbAccess.ExecuteNonQuery("dbo.sStatusGet", ref parameters, CommandType.StoredProcedure);

            if (parameters.GetBool("@result"))
            {
                var status = new Status();
                status.Vin = parameters.GetString("@vin");
                status.Lock = parameters.GetBool("@lock");
                status.Battery = parameters.GetInt("@battery");
                status.Gps_Longitude = parameters.GetDouble("@gps_longitude");
                status.Gps_Latitude = parameters.GetDouble("@gps_latitude");
                status.Alarm = parameters.GetBool("@alarm");
                status.TirePressure = parameters.GetString("@tirepressure");
                status.Milage = parameters.GetDouble("@milage");
                status.DateLastModified = parameters.GetDateTime("@DateLastModified");

                return status;
            }
            return null;
        }

        public void UpdateSummaryStatusInDB(Status status, string vin)
        {
            var parameters = new SqlParameters();
            parameters.AddNVarChar("@vin", 50, vin);
            parameters.AddBoolean("@lock", status.Lock, ParameterDirection.Input);
            parameters.AddInt("@battery", status.Battery, ParameterDirection.Input);
            parameters.AddFloat("@gps_longitude", status.Gps_Longitude, ParameterDirection.Input);
            parameters.AddFloat("@gps_latitude", status.Gps_Latitude, ParameterDirection.Input);
            parameters.AddBoolean("@alarm", status.Alarm, ParameterDirection.Input);
            parameters.AddVarChar("@tirepressure", 50, status.TirePressure, ParameterDirection.Input);
            parameters.AddFloat("@milage", status.Milage, ParameterDirection.Input);
            DbAccess.ExecuteNonQuery("dbo.sStatusUpdate", ref parameters, CommandType.StoredProcedure);
        }

        public void UpdateAlarmStatusInDB(string vin, bool alarm)
        {
            var status = GetStatus(vin);
            status.Alarm = alarm;
            UpdateSummaryStatusInDB(status, vin);
        }

        public void UpdateMilageStatusInDB(string vin, float milage)
        {
            var status = GetStatus(vin);
            status.Milage = milage;
            UpdateSummaryStatusInDB(status, vin);
        }

        public void UpdateTirePressureStatusInDB(string vin, string pressure)
        {
            var status = GetStatus(vin);
            status.TirePressure = pressure;
            UpdateSummaryStatusInDB(status, vin);
        }

        public void UpdateGpsStatusInDB(string vin, double longitude, double latitude)
        {
            var status = GetStatus(vin);
            status.Gps_Latitude = latitude;
            status.Gps_Longitude = longitude;
            UpdateSummaryStatusInDB(status, vin);
        }
        public void UpdateLockStatusInDB(string vin, bool lockStatus)
        {
            var status = GetStatus(vin);
            status.Lock = lockStatus;
            UpdateSummaryStatusInDB(status, vin);
        }
        public void UpdateBatteryStatusInDB(string vin, int BatteryLevel)
        {
            var status = GetStatus(vin);
            status.Battery = BatteryLevel;
            UpdateSummaryStatusInDB(status, vin);
        }
        public string SearchVehicle(string vin)
        {
            if (!string.IsNullOrEmpty(vin))
            {
                var parameters = new SqlParameters();
                parameters.AddNVarChar("@vin", 50, vin);
                parameters.AddNVarChar("@regNo", 50, "", ParameterDirection.Output);
                parameters.AddBoolean("@Result", false, ParameterDirection.Output);
                DbAccess.ExecuteNonQuery("dbo.sGetRegNo", ref parameters, CommandType.StoredProcedure);
                if (parameters.GetBool("@Result"))
                {
                    var regNo = parameters.GetString("@regNo");
                    return regNo;
                }
            }
            var response = "Invalid vin input";
            return response;
        }
        public bool CheckIfCarHasAnOwnerInCDS(string vin, string authToken)
        {
            var regNo = SearchVehicle(vin);
            if (regNo.ToLower().Equals("invalid vin input"))
            {
                return false;
            }

            var responseFromCDS = _cdsClient.ownerData(regNo, vin, authToken);
            if (responseFromCDS.owner == null)
            {
                return false;
            } else
            {
                return true;
            }
        }
        public Guid CreateVehicle(Vehicle vehicle)
        {
            var parameters = new SqlParameters();
            
            parameters.AddNVarChar("@vin", 50, vehicle.Vin);
                       
            if (vehicle.owner != null && vehicle.owner.Id != Guid.Empty)
            {
                parameters.AddUniqueIdentifier("@id", vehicle.owner.Id, ParameterDirection.Input);
                parameters.AddNVarChar("@displayName", vehicle.owner.FirstName, 50, ParameterDirection.Input);
            }
            else
            {
                parameters.AddUniqueIdentifier("@id", Guid.Empty, ParameterDirection.Input);
                parameters.AddNVarChar("@displayName", null, 50, ParameterDirection.Input);
            }
            parameters.AddNVarChar("@regNo", 50, vehicle.Regno);

            DbAccess.ExecuteNonQuery("dbo.sAddVehicle", ref parameters, CommandType.StoredProcedure);

            return new Guid();
        }
        public Guid UpdateVinList(Vehicle vehicle)
        {
            var parameters = new SqlParameters();
            parameters.AddNVarChar("@vin", 50, vehicle.Vin);
            if (vehicle.owner != null && vehicle.owner.Id != Guid.Empty)
            {
                parameters.AddUniqueIdentifier("@id", vehicle.owner.Id, ParameterDirection.Input);
                parameters.AddNVarChar("@displayName", vehicle.owner.FirstName, 50, ParameterDirection.Input);
            }
            else
            {
                parameters.AddUniqueIdentifier("@id", Guid.Empty,  ParameterDirection.Input);
                parameters.AddNVarChar("@displayName", null, 50, ParameterDirection.Input);
            }
            parameters.AddNVarChar("@regNo", 50, vehicle.Regno);

            DbAccess.ExecuteNonQuery("dbo.sUpdateVehicleList", ref parameters, CommandType.StoredProcedure);

            return new Guid();
        }
        public Destination GetNewRoutesDestination(string vin)
        {
            Destination destination = new Destination();
 
            var parameters = new SqlParameters();
            parameters.AddNVarChar("@vin", 50, vin);
            parameters.AddBoolean("@result", false, ParameterDirection.Output);
            parameters.AddFloat("@latitude", 0, ParameterDirection.Output);
            parameters.AddFloat("@longitude", 0, ParameterDirection.Output);
            parameters.AddDateTime("@timestamp", DateTime.Now, ParameterDirection.Output);

            DbAccess.ExecuteNonQuery("dbo.sGetDestination", ref parameters, CommandType.StoredProcedure);

            if (parameters.GetBool("@result"))
            {
                destination.Latitude = parameters.GetDouble("@latitude");
                destination.Longitude = parameters.GetDouble("@longitude");
                destination.Timestamp = parameters.GetDateTime("@timestamp");
            }

            return destination;
        }
    }
}
