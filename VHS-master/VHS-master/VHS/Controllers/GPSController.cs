using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VHS.Entity;
using VHSBackend.Core.Integrations;
using VHSBackend.Core.Repository;

namespace VHSBackend.Web.Controllers
{
    [Route("api/route")]
    [ApiController]
    public class GPSController : ControllerBase
    {
        public GPSController()
        {
            _sqlStatusRepository = new SqlGPSRepository();
            _cDSUserRepository = new CDSUserRepository();
            _sqlVehicleRepository = new SqlVehicleRepository();
            _sqlGPSRepository = new SqlGPSRepository();
        }
        private readonly SqlGPSRepository _sqlStatusRepository;
        private readonly CDSUserRepository _cDSUserRepository;
        private readonly SqlVehicleRepository _sqlVehicleRepository;
        private readonly SqlGPSRepository _sqlGPSRepository;

        Status status = new Status();

        [HttpPost]
        [Route("{vin}/destination")]
        public ActionResult<string> SendDestinationToVehicle(string vin, string userName, string password, double longitude, double latitude, string authToken)
        {

            if(_cDSUserRepository.ValidateUsersCarOwnershipInCDS(userName, password, vin, authToken))
            {
                    if (longitude is double && latitude != null)
                    {
                        _sqlStatusRepository.logDestinationInRouteTable(vin, longitude, latitude);

                        return new OkObjectResult("New destination send to DB");
                    }
                    return new BadRequestObjectResult(" missing values?");

            }
            return new UnauthorizedObjectResult("Unauthorized user access to CDS");
        }

        [HttpGet]
        [Route("{vin}/destination")]
        public ActionResult<string> GetDestinationsFromDB(string vin)
        {
            if (vin != null)
            {
                Destination destination = _sqlVehicleRepository.GetNewRoutesDestination(vin);
                return new OkObjectResult(destination);
            }
            return new NotFoundObjectResult("Nothing to return");
        }

        [HttpPatch]
        [Route("{vin}/destination")]
        public ActionResult<bool> UpdateDestinationInDB(string vin, double latitude, double longitude)
        {
            if (vin != null)
            {
                _sqlStatusRepository.updateDestinationInRouteTable(vin, longitude, latitude);
                return new OkObjectResult("Updated!");
            }
            return new BadRequestObjectResult("Wrong input");
        }

        [HttpDelete]
        [Route("{vin}/destination")]
        public ActionResult<bool> DeleteDestinationInDB(string vin)
        {
            _sqlStatusRepository.DeleteDestinationFromDB(vin);
            return true;
        }
    }
}
