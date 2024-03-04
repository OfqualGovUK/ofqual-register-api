using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Repository
{
    public interface IRegisterRepository
    {
        Task<object> GetDataAsync(string key);
        public Task<IEnumerable<Organisation>> GetOrganisations();
        public Task<IEnumerable<Qualification>> GetQualifications();
    }
}
