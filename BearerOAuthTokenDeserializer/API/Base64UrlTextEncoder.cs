using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearerOAuthTokenDeserializer
{
    public class Base64UrlTextEncoder : ITextEncoder
    {
        public Base64UrlTextEncoder()
        {
        }

        public byte[] Decode(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            return Convert.FromBase64String(Base64UrlTextEncoder.Pad(text.Replace('-', '+').Replace('\u005F', '/')));
        }

        public string Encode(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            string base64String = Convert.ToBase64String(data);
            char[] chrArray = new char[] { '=' };
            return base64String.TrimEnd(chrArray).Replace('+', '-').Replace('/', '\u005F');
        }

        private static string Pad(string text)
        {
            int length = 3 - (text.Length + 3) % 4;
            if (length == 0)
            {
                return text;
            }
            return string.Concat(text, new string('=', length));
        }
    }
}
