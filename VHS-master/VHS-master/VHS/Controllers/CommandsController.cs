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
    [Route("api/commands")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        public CommandsController()
        {
            _cdsClient = new CdsClient();
            _sqlVehicleRepository = new SqlVehicleRepository();
            _cDSUserRepository = new CDSUserRepository();
            _sqlCommandRepository = new SqlCommandRepository();
        }
        private readonly CdsClient _cdsClient;
        private readonly SqlVehicleRepository _sqlVehicleRepository;
        private readonly CDSUserRepository _cDSUserRepository;
        private readonly SqlCommandRepository _sqlCommandRepository;


        [HttpPost]
        [Route("{vin}")]
        public ActionResult<string> SendCommand(string vin, string userName, string password, Command command, string authToken)
        {
            if (_cDSUserRepository.ValidateUsersCarOwnershipInCDS(userName, password, vin, authToken))
            {

                _sqlCommandRepository.UpdateCommandInDB(vin, command);

                return new OkObjectResult("You have a Car");
            }
            return new BadRequestObjectResult("wrong car ! ");
        }

        [HttpGet]
        [Route("{vin}")]
        public ActionResult<string> GetCommand(string vin)
        {
            var response = _sqlCommandRepository.GetCommand(vin);

            return new OkObjectResult(response);
        }
        // Post endpoint for car to confirm actions execution
        [HttpPost]
        [Route("{vin}/reset")]
        public ActionResult<string> ResetCommand(string vin)
        {
            // metod som resetar alla kommando till 0
            _sqlCommandRepository.ResetCommandInDB(vin);
            

            return new OkObjectResult("Reseted ! ");
        }

        [HttpPost]
        [Route("{vin}/findVehicleByBarking")]
        public ActionResult<bool> VehicleBark(string vin, float Latitute, float Longitude)
        {
            if(_sqlCommandRepository.VehicleBarkCommandInDB(vin, Latitute, Longitude))
            {
                return new OkObjectResult("Search your Car ! ");
            }
            return new NotFoundObjectResult("you are not in range! ");
            
        }
    }
}
