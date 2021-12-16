using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace VHSBackend.Core.Configurations
{
    public class ConnectionStringsConfiguration : AConfigBase
    {

        public ConnectionStringsConfiguration(IConfigurationRoot configurationRoot) : base(configurationRoot)
        {
            _root = "ConnectionStrings";
        }

        private readonly string _root;
        public string VHSDbConnectionString
        {
            get { return base.GetConfigValue(_root, "VHSDb"); }
        }
    }
}
