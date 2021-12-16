using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace VHSBackend.Core.Configurations
{
    public abstract class AConfigBase
    {

        protected AConfigBase(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        protected IConfigurationRoot _configurationRoot;

        protected string GetConfigValue(string root, string subPath)
        {
            return GetConfigValue(new string[2] { root, subPath });
        }

        protected string GetConfigValue(string[] paths)
        {

            var p = string.Empty;
            foreach (var path in paths)
            {
                if (p.Length > 0)
                {
                    p = $"{p}:";
                }

                p = $"{p}{path}";
            }

            return _configurationRoot[p];
        }
    }

    
}
