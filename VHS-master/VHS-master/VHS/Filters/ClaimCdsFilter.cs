using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VHSBackend.Core;
using VHSBackend.Core.Integrations;

namespace VHSBackend.Web.Filters
{
    public class ClaimCdsFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
           
            var hasClaim = false;

            var authHeader = context.HttpContext.Request.Headers["Authorization"];

            if (authHeader.Count == 1)
            {
                var token = authHeader[0];
                var cdsClient = new CdsClient();
                var userId = ServiceProvider.Current.InMemoryStorage.GetUserId(token);
                hasClaim = cdsClient.ValidateToken(userId, token);


                cdsClient = null;
            }


            if (!hasClaim)
            {
                context.Result = new UnauthorizedResult();
            }

        }
    }
}
