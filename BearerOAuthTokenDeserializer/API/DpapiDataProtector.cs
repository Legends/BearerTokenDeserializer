using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BearerOAuthTokenDeserializer
{
    internal class DpapiDataProtectorWrapper : IDataProtector
    {
        private readonly DpapiDataProtector _protector;

        public DpapiDataProtectorWrapper(string appName, string[] purposes)
        {
            DpapiDataProtector dpapiDataProtector = new DpapiDataProtector(appName, "Microsoft.Owin.Security.IDataProtector", purposes)
            {
                Scope = DataProtectionScope.CurrentUser
            };
            this._protector = dpapiDataProtector;
        }

        public byte[] Protect(byte[] userData)
        {
            return this._protector.Protect(userData);
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return this._protector.Unprotect(protectedData);
        }
    }

    public interface IDataProtector
    {
        /// <summary>
        /// Called to protect user data.
        /// </summary>
        /// <param name="userData">The original data that must be protected</param>
        /// <returns>A different byte array that may be unprotected or altered only by software that has access to 
        /// the an identical IDataProtection service.</returns>
        byte[] Protect(byte[] userData);

        /// <summary>
        /// Called to unprotect user data
        /// </summary>
        /// <param name="protectedData">The byte array returned by a call to Protect on an identical IDataProtection service.</param>
        /// <returns>The byte array identical to the original userData passed to Protect.</returns>
        byte[] Unprotect(byte[] protectedData);
    }
}
