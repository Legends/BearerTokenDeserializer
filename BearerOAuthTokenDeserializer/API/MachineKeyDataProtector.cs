using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace BearerOAuthTokenDeserializer
{
    internal class MachineKeyDataProtector : IDataProtector
    {
        private readonly string[] _purposes;

        public MachineKeyDataProtector(params string[] purposes)
        {
            this._purposes = purposes;
        }

        public virtual byte[] Protect(byte[] userData)
        {
            return MachineKey.Protect(userData, this._purposes);
        }

        public virtual byte[] Unprotect(byte[] protectedData)
        {
            return MachineKey.Unprotect(protectedData, this._purposes);
        }
    }
}
