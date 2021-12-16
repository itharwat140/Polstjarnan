using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VHSBackend.Web.Filters;

namespace VHSBackend.Web.Attributes
{
    public class CdsAuthorizeAttribute : TypeFilterAttribute
    {
        public CdsAuthorizeAttribute() : base(typeof(ClaimCdsFilter))
        {
        }
    }
}
