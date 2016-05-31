using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearerOAuthTokenDeserializer
{
    public class AuthenticationProperties
    {
        internal const string IssuedUtcKey = ".issued";

        internal const string ExpiresUtcKey = ".expires";

        internal const string IsPersistentKey = ".persistent";

        internal const string RedirectUriKey = ".redirect";

        internal const string RefreshKey = ".refresh";

        internal const string UtcDateTimeFormat = "r";

        private readonly IDictionary<string, string> _dictionary;

        /// <summary>
        /// Gets or sets if refreshing the authentication session should be allowed.
        /// </summary>
        public bool? AllowRefresh
        {
            get
            {
                string str;
                bool flag;
                if (this._dictionary.TryGetValue(".refresh", out str) && bool.TryParse(str, out flag))
                {
                    return new bool?(flag);
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    IDictionary<string, string> str = this._dictionary;
                    bool flag = value.Value;
                    str[".refresh"] = flag.ToString(CultureInfo.InvariantCulture);
                    return;
                }
                if (this._dictionary.ContainsKey(".refresh"))
                {
                    this._dictionary.Remove(".refresh");
                }
            }
        }

        /// <summary>
        /// State values about the authentication session.
        /// </summary>
        public IDictionary<string, string> Dictionary
        {
            get
            {
                return this._dictionary;
            }
        }

        /// <summary>
        /// Gets or sets the time at which the authentication ticket expires.
        /// </summary>
        public DateTimeOffset? ExpiresUtc
        {
            get
            {
                string str;
                DateTimeOffset dateTimeOffset;
                if (this._dictionary.TryGetValue(".expires", out str) && DateTimeOffset.TryParseExact(str, "r", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dateTimeOffset))
                {
                    return new DateTimeOffset?(dateTimeOffset);
                }
                return null;
            }
            set
            {
                if (!value.HasValue)
                {
                    if (this._dictionary.ContainsKey(".expires"))
                    {
                        this._dictionary.Remove(".expires");
                    }
                    return;
                }
                IDictionary<string, string> str = this._dictionary;
                DateTimeOffset dateTimeOffset = value.Value;
                str[".expires"] = dateTimeOffset.ToString("r", CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets or sets whether the authentication session is persisted across multiple requests.
        /// </summary>
        public bool IsPersistent
        {
            get
            {
                return this._dictionary.ContainsKey(".persistent");
            }
            set
            {
                if (this._dictionary.ContainsKey(".persistent"))
                {
                    if (!value)
                    {
                        this._dictionary.Remove(".persistent");
                        return;
                    }
                }
                else if (value)
                {
                    this._dictionary.Add(".persistent", string.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the time at which the authentication ticket was issued.
        /// </summary>
        public DateTimeOffset? IssuedUtc
        {
            get
            {
                string str;
                DateTimeOffset dateTimeOffset;
                if (this._dictionary.TryGetValue(".issued", out str) && DateTimeOffset.TryParseExact(str, "r", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dateTimeOffset))
                {
                    return new DateTimeOffset?(dateTimeOffset);
                }
                return null;
            }
            set
            {
                if (!value.HasValue)
                {
                    if (this._dictionary.ContainsKey(".issued"))
                    {
                        this._dictionary.Remove(".issued");
                    }
                    return;
                }
                IDictionary<string, string> str = this._dictionary;
                DateTimeOffset dateTimeOffset = value.Value;
                str[".issued"] = dateTimeOffset.ToString("r", CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets or sets the full path or absolute URI to be used as an http redirect response value. 
        /// </summary>
        public string RedirectUri
        {
            get
            {
                string str;
                if (!this._dictionary.TryGetValue(".redirect", out str))
                {
                    return null;
                }
                return str;
            }
            set
            {
                if (value != null)
                {
                    this._dictionary[".redirect"] = value;
                    return;
                }
                if (this._dictionary.ContainsKey(".redirect"))
                {
                    this._dictionary.Remove(".redirect");
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.Owin.Security.AuthenticationProperties" /> class
        /// </summary>
        public AuthenticationProperties()
        {
            this._dictionary = new Dictionary<string, string>(StringComparer.Ordinal);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.Owin.Security.AuthenticationProperties" /> class
        /// </summary>
        /// <param name="dictionary"></param>
        public AuthenticationProperties(IDictionary<string, string> dictionary)
        {
            object strs = dictionary;
            if (strs == null)
            {
                strs = new Dictionary<string, string>(StringComparer.Ordinal);
            }
            this._dictionary = (IDictionary<string, string>) strs;
        }
    }
}
