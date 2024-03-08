using Ofqual.Common.RegisterAPI.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Services.Database
{
    public interface IRegisterDb
    {
        public Task<List<Organisation>> GetOrganisations();
        public Task<List<Qualification>> GetQualifications(string search = "");
        public Task<List<QualificationPublic>> GetQualificationsPublic(string search = "");

    }
}
