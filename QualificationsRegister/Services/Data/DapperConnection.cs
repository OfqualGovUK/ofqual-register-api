using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Services.Data
{
    public class DapperDbConnection : IDapperDbConnection
    {
        public readonly string _connectionString;

        public DapperDbConnection(IConfiguration configuration)
        {
            _connectionString = Environment.GetEnvironmentVariable("MDDBConnString");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

    }
}
