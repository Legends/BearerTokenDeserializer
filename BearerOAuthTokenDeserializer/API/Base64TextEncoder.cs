using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearerOAuthTokenDeserializer
{
    public class Base64TextEncoder : ITextEncoder
    {
        public Base64TextEncoder()
        {
        }

        public byte[] Decode(string text)
        {
            return Convert.FromBase64String(text);
        }

        public string Encode(byte[] data)
        {
            return Convert.ToBase64String(data);
        }
    }

    public interface ITextEncoder
    {
        byte[] Decode(string text);

        string Encode(byte[] data);
    }
}
