using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearerOAuthTokenDeserializer
{
    public class DataProtector
    {
        // IIS will use ASP.NET machine key data protection
        // HttpListener and other self - hosted servers will use DPAPI data protection
        public static SecureDataFormat<AuthenticationTicket> Create()
        {
            string[] purposes = new string[] { typeof(OAuthBearerAuthenticationMiddleware).Namespace, "Access_Token", "v1" };
            var dp = new MachineKeyDataProtector(purposes);
            var enc = new Base64UrlTextEncoder();
            var ts = new TicketSerializer();
            var sdf = new SecureDataFormat<AuthenticationTicket>(ts, dp, enc);
            return sdf;
        }
    }
}
