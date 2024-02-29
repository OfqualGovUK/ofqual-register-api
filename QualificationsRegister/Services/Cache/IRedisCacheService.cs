using Ofqual.Common.RegisterAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Services.Cache
{
    public interface IRedisCacheService
    {
        public Task<List<T>> GetCacheAsync<T>(string key);
    }
}
