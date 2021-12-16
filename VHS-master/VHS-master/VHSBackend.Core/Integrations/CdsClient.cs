using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using RestSharp;
using VHS.Entity;
using VHS.Entity.Cds;

namespace VHSBackend.Core.Integrations
{
    public class CdsClient
    {
        private readonly IRestClient _restClient;

        public CdsClient()
        {
            _restClient = new RestClient("https://kyhdev.hiqcloud.net");
        }

        public IList<Vehicle> listVins(string regNo, string authToken)
        {
            var request = new RestRequest($"/api/cds/v1.0/vehicle/list", Method.GET);
            if (regNo == null)
            {
                return Execute<IList<Vehicle>>(request, authToken);
            }

            request = new RestRequest($"/api/cds/v1.0/vehicle/list?regNo={HttpUtility.UrlEncode(regNo)}&kyh-auth:{HttpUtility.UrlEncode("das8783nmncxzJJDKnknxz48ZMCCMKJKERK29489u5nknxC")}", Method.GET);
            return Execute<IList<Vehicle>>(request, authToken);

        }

        public Vehicle ownerData(string regNo, string vin, string authToken)
        {
            var request = new RestRequest($"/api/cds/v1.0/vehicle/{vin}/{regNo}", Method.GET);

            //request.AddHeader("kyh-auth", authToken);
            //var response = _restClient.Execute(request);
            //if (response.IsSuccessful)
            //{
            //    return (Vehicle)JsonConvert.DeserializeObject(response.Content);
            //}

            return Execute<Vehicle> (request, authToken);
        }

        public LoginResponse Login(string userName, string password)
        {
            var request =
                new RestRequest(
                    $"api/cds/v1.0/user/authenticate?userName={HttpUtility.UrlEncode(userName)}&pwd={HttpUtility.UrlEncode(password)}",
                    Method.GET);
            return Execute<LoginResponse>(request, "das8783nmncxzJJDKnknxz48ZMCCMKJKERK29489u5nknxC");
        }

        public bool ValidateToken(Guid userId, string token)
        {
            var request =
                new RestRequest($"/api/cds/v1.0/user/{userId}/validate?token={HttpUtility.UrlEncode(token)}");

            return Execute<bool>(request, " ");
        }
        private T Execute<T>(IRestRequest request, string token)
        {
            request.AddHeader("kyh-auth", token);
            var response = _restClient.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<T>(response.Content);
                return (T)(object)result;
            }

            return default(T);

        }

    }
}
