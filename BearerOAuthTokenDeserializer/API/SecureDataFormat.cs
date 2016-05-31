using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearerOAuthTokenDeserializer
{
    public class SecureDataFormat<TData> : ISecureDataFormat<TData>
    {
        private readonly IDataSerializer<TData> _serializer;

        private readonly IDataProtector _protector;

        private readonly ITextEncoder _encoder;

        public SecureDataFormat(IDataSerializer<TData> serializer, IDataProtector protector, ITextEncoder encoder)
        {
            this._serializer = serializer;
            this._protector = protector;
            this._encoder = encoder;
        }

        public string Protect(TData data)
        {
            byte[] numArray = this._serializer.Serialize(data);
            byte[] numArray1 = this._protector.Protect(numArray);
            return this._encoder.Encode(numArray1);
        }

        public TData Unprotect(string protectedText)
        {
            TData tDatum;
            try
            {
                if (protectedText != null)
                {
                    byte[] numArray = this._encoder.Decode(protectedText);
                    if (numArray != null)
                    {
                        byte[] numArray1 = this._protector.Unprotect(numArray);
                        tDatum = (numArray1 != null ? this._serializer.Deserialize(numArray1) : default(TData));
                    }
                    else
                    {
                        tDatum = default(TData);
                    }
                }
                else
                {
                    tDatum = default(TData);
                }
            }
            catch (System.Exception ex)
            {
                tDatum = default(TData);
            }
            return tDatum;
        }
    }

    public interface ISecureDataFormat<TData>
    {
        string Protect(TData data);

        TData Unprotect(string protectedText);
    }
}
