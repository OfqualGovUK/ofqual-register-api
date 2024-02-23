using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Services.Data
{
    public interface IDapperDbConnection
    {
        public IDbConnection CreateConnection();
    }
}
