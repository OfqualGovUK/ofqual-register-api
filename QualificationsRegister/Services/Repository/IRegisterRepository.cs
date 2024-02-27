using Ofqual.Common.RegisterAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Services.Repository
{
    public interface IRegisterRepository
    {
        public Dictionary<string, object> GetData();
    }
}
