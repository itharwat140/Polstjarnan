using System;
using System.Collections.Generic;
using System.Text;

namespace VHS.Entity.Cds
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }
}
