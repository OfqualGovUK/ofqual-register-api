using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualificationsRegister.Gateways

{
    public interface IOrganisationsGateway
    {
        Task<List<string>> GetOrganisations(string organisationName);
        Task<string> GetOrganisationByNumber(string organisationNum);
    }
}
