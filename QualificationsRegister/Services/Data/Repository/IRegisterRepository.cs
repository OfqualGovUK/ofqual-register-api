using Ofqual.Common.RegisterAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Services.Data.Repository
{
    public interface IRegisterRepository
    {
        public Task<Dictionary<string, object>> GetDataAsync();
    }
}
