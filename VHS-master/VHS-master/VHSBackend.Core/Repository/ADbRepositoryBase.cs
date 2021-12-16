using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiQ.NetStandard.Util.Data;

namespace VHSBackend.Core.Repository
{
    public abstract class ADbRepositoryBase
    {
        private readonly SqlDbAccess _sqlDbAccess;

        protected ADbRepositoryBase()
        {
            _sqlDbAccess =
                new SqlDbAccess(ServiceProvider.Current.Configuration.ConnectionStrings.VHSDbConnectionString);
        }

        protected SqlDbAccess DbAccess
        {
            get { return _sqlDbAccess; }
        }
    }

    

}
