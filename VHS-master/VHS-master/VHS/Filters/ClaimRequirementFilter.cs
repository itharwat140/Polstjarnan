using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Security.Claims;
using VHS.Entity;
using VHSBackend.Core.Repository;

namespace VHSBackend.Web.Filters
{
    //public class ClaimRequirementFilter : IAuthorizationFilter
    //{
    //    public ClaimRequirementFilter(Claim claim)
    //    {
    //        _claim = claim;
    //        _userRepository = new SqlUserRepository();
    //    }

    //    private readonly Claim _claim;
    //    private readonly IUserRepository _userRepository;

    //    public void OnAuthorization(AuthorizationFilterContext context)
    //    {
    //        var hasClaim = false;

    //        var claimTypes = JsonConvert.DeserializeObject<ClaimTypeCollection>(_claim.Value);
    //        if (claimTypes != null)
    //        {
    //            if (claimTypes.GetValue<bool>("requiretoken"))
    //            {
    //                var authHeader = context.HttpContext.Request.Headers["trashvac-auth"];
    //                if (authHeader.Count == 1)
    //                {
    //                    var token = authHeader[0];
    //                    hasClaim = _userRepository.ValidateToken(token, out var user);
    //                    var minReqUserLevel = claimTypes.GetValue<Enums.UserLevel>("minuserlevel");
    //                    if (minReqUserLevel >= Enums.UserLevel.Undefined)
    //                    {
    //                        hasClaim = user.UserLevel >= minReqUserLevel;
    //                    }
    //                }
    //            }
    //        }

            
    //        if (!hasClaim)
    //        {
    //            context.Result = new UnauthorizedResult();
    //        }

    //    }
    //}
}
