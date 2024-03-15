using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Models.Response
{
    public class ListOrganisationsResponse
    {
        public List<Organisation>? Organisations { get; set; }
        public string? NextPage { get; set; }
    }
}
