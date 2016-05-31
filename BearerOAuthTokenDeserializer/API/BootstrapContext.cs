using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.IdentityModel.Tokens;
using System.IdentityModel.Configuration;
using System.IdentityModel.Diagnostics.Application;
using System.IdentityModel.Selectors;

namespace BearerOAuthTokenDeserializer
{
    [Serializable]
    public class BootstrapContext : ISerializable
    {
        private SecurityToken _token;

        private string _tokenString;

        private byte[] _tokenBytes;

        private SecurityTokenHandler _tokenHandler;

        private const string _tokenTypeKey = "K";

        private const string _tokenKey = "T";

        private const char _securityTokenType = 'T';

        private const char _stringTokenType = 'S';

        private const char _byteTokenType = 'B';

        public SecurityToken SecurityToken
        {
            get
            {
                return this._token;
            }
        }

        public SecurityTokenHandler SecurityTokenHandler
        {
            get
            {
                return this._tokenHandler;
            }
        }

        public string Token
        {
            get
            {
                return this._tokenString;
            }
        }

        public byte[] TokenBytes
        {
            get
            {
                return this._tokenBytes;
            }
        }

        protected BootstrapContext(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                return;
            }
            char chr = info.GetChar("K");
            if (chr == 'B')
            {
                this._tokenBytes = (byte[]) info.GetValue("T", typeof(byte[]));
            }
            else
            {
                if (chr == 'S')
                {
                    this._tokenString = info.GetString("T");
                    return;
                }
                if (chr == 'T')
                {
                    SecurityTokenHandler securityTokenHandler = context.Context as SecurityTokenHandler;
                    if (securityTokenHandler == null)
                    {
                        this._tokenString = Encoding.UTF8.GetString(Convert.FromBase64String(info.GetString("T")));
                        return;
                    }
                    using (XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateTextReader(Convert.FromBase64String(info.GetString("T")), XmlDictionaryReaderQuotas.Max))
                    {
                        xmlDictionaryReader.MoveToContent();
                        if (securityTokenHandler.CanReadToken(xmlDictionaryReader))
                        {
                            string localName = xmlDictionaryReader.LocalName;
                            string namespaceURI = xmlDictionaryReader.NamespaceURI;
                            SecurityToken securityToken = securityTokenHandler.ReadToken(xmlDictionaryReader);
                            if (securityToken != null)
                            {
                                this._token = securityToken;
                            }
                            else
                            {
                                this._tokenString = Encoding.UTF8.GetString(Convert.FromBase64String(info.GetString("T")));
                                return;
                            }
                        }
                    }
                }
            }
        }

        public BootstrapContext(SecurityToken token, SecurityTokenHandler tokenHandler)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }
            if (tokenHandler == null)
            {
                throw new ArgumentNullException("tokenHandler");
            }
            this._token = token;
            this._tokenHandler = tokenHandler;
        }

        public BootstrapContext(string token)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }
            this._tokenString = token;
        }

        public BootstrapContext(byte[] token)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }
            this._tokenBytes = token;
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (this._tokenBytes != null)
            {
                info.AddValue("K", 'B');
                info.AddValue("T", this._tokenBytes);
                return;
            }
            if (this._tokenString != null)
            {
                info.AddValue("K", 'S');
                info.AddValue("T", this._tokenString);
                return;
            }
            if (this._token != null && this._tokenHandler != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    info.AddValue("K", 'T');
                    using (XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateTextWriter(memoryStream, Encoding.UTF8, false))
                    {
                        this._tokenHandler.WriteToken(xmlDictionaryWriter, this._token);
                        xmlDictionaryWriter.Flush();
                        info.AddValue("T", Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int) memoryStream.Length));
                    }
                }
            }
        }
    }
}
