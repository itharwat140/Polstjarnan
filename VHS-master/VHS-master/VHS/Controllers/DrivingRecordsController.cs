using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VHS.Entity;
using VHSBackend.Core.Repository;

namespace VHSBackend.Web.Controllers
{
    [Route("api/drivingrecords")]
    [ApiController]
    public class DrivingRecordsController : ControllerBase
    {
        public DrivingRecordsController()
        {
            _sqlDrivingRecordsRepository = new SqlDrivingRecordsRepository();
            _sqlVehicleRepository = new SqlVehicleRepository();
        }

        private readonly SqlDrivingRecordsRepository _sqlDrivingRecordsRepository;
        private readonly SqlVehicleRepository _sqlVehicleRepository;
        
        
        [HttpPost]
        [Route("{vin}/startTrip")]
        public ActionResult<Guid> StartJournal(string vin, string authToken)
        {
            
            if (_sqlVehicleRepository.CheckIfCarHasAnOwnerInCDS(vin, authToken))
            {
                Guid journal_id = _sqlDrivingRecordsRepository.StartDrivingJournal(vin);
                return new OkObjectResult(journal_id);
            }
            return new NotFoundObjectResult("have not access ! ");
        }

        [HttpPost]
        [Route("{vin}/{journal_id}/triplogs")]
        public ActionResult<bool> sendRegularLogsUnderTrip(string vin, Guid journal_id, DriveLogData logData)
        {
            _sqlDrivingRecordsRepository.sendDrivingLogs(vin, journal_id, logData);

            return new OkObjectResult(true);
        }

        [HttpPost]
        [Route("{vin}/{journal_id}/savetrip")]
        public ActionResult<IList<DriveLogData>>  SaveTrip(string vin, Guid journal_id)
        {

            IList<DriveLogData> tripLogs = _sqlDrivingRecordsRepository.GetTripLogs(vin, journal_id);
            

            return new OkObjectResult(tripLogs);
        }
    }
}
