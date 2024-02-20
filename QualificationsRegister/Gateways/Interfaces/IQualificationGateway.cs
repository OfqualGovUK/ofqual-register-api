using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualificationsRegister.Gateways
{
    public interface IQualificationGateway
    {
        Task<List<string>> GetQualifications();
    }
}
