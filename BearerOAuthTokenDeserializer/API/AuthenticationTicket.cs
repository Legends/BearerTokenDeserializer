using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BearerOAuthTokenDeserializer
{
    public class AuthenticationTicket
    {
        /// <summary>
        /// Gets the authenticated user identity.
        /// </summary>
        public ClaimsIdentity Identity
        {
            get;
            private set;
        }

        /// <summary>
        /// Additional state values for the authentication session.
        /// </summary>
        public AuthenticationProperties Properties
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.Owin.Security.AuthenticationTicket" /> class
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="properties"></param>
        public AuthenticationTicket(ClaimsIdentity identity, AuthenticationProperties properties)
        {
            this.Identity = identity;
            this.Properties = properties ?? new AuthenticationProperties();
        }
    }
}
