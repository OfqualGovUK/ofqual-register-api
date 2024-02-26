using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.Private;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Services.Repository
{
    public interface IRegisterRepository
    {
        public Task<Dictionary<string, object>> GetDataAsync();
        public Task<IEnumerable<OrganisationPrivate>> GetOrganisations();
        //public Task GetQualifications();
    }
}
