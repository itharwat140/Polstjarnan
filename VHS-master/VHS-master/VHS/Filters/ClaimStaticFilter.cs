using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VHSBackend.Core;

namespace VHSBackend.Web.Filters
{
    public class ClaimStaticFilter : IAuthorizationFilter
    {

        public ClaimStaticFilter(Claim claim)
        {
            _claim = claim;
        }
        private readonly Claim _claim;  

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasClaim = false;

            var authHeader = context.HttpContext.Request.Headers["trashvac-auth"];
            if (authHeader.Count == 1)
            {
                var token = authHeader[0];
                hasClaim = token.Equals(ServiceProvider.Current.Configuration.SiteSettings.StaticAccessToken,
                    StringComparison.Ordinal);
            }

            if (!hasClaim)
            {
                context.Result = new UnauthorizedResult();
            }
        }

    }
}
