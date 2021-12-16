using System;
using System.Collections.Generic;
using System.Text;
using VHSBackend.Core;

namespace VHSBackend.Core
{
    public class ServiceProvider
    {
        private static Services _services;

        public static Services Current => _services ??= new Services();

        public static Services Current2
        {
            get
            {
                if (_services == null)
                {
                    _services = new Services();
                }

                return _services;
            }
        }
    }
}