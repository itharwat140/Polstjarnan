using System;
using System.Collections.Generic;
using System.Text;
using VHSBackend.Core.Integrations;

namespace VHSBackend.Core.Repository
{
    public class CDSUserRepository
    {
        public CDSUserRepository()
        {
            _cdsClient = new CdsClient();
            _sqlVehicleRepository = new SqlVehicleRepository();
        }
    
        private readonly CdsClient _cdsClient;
        private readonly SqlVehicleRepository _sqlVehicleRepository;

        public bool ValidateUsersCarOwnershipInCDS(string userName, string password, string vin, string authToken)
        {
            var result = _cdsClient.Login(userName, password);

            if (result != null)
            {
                Guid id = result.Id;
                var response = _cdsClient.ValidateToken(id, result.AccessToken);
                if (_sqlVehicleRepository.CheckIfCarHasAnOwnerInCDS(vin, authToken))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
