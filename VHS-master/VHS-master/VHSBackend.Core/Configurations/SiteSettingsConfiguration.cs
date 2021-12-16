using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace VHSBackend.Core.Configurations
{
    public class SiteSettingsConfiguration : AConfigBase
    {
        public SiteSettingsConfiguration(IConfigurationRoot configurationRoot) : base(configurationRoot)
        {
            _root = "SiteSettings";
        }

        private readonly string _root;

        public int AccessTokenTtl
        {
            get { return Convert.ToInt32(base.GetConfigValue(_root, "AccessTokenTTL")); }
        }
        public string StaticAccessToken
        {
            get { return base.GetConfigValue(_root, "StaticAccessToken"); }
        }
    }
}
