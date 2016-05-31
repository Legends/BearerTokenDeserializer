using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BearerOAuthTokenDeserializer
{
    public class TicketSerializer : IDataSerializer<AuthenticationTicket>
    {
        private const int FormatVersion = 3;

        public TicketSerializer()
        {
        }

        public virtual AuthenticationTicket Deserialize(byte[] data)
        {
            AuthenticationTicket authenticationTicket;
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    using (BinaryReader binaryReader = new BinaryReader(gZipStream))
                    {
                        authenticationTicket = TicketSerializer.Read(binaryReader);
                    }
                }
            }
            return authenticationTicket;
        }

        public static AuthenticationTicket Read(BinaryReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            if (reader.ReadInt32() != 3)
            {
                return null;
            }
            string str = reader.ReadString();
            string str1 = TicketSerializer.ReadWithDefault(reader, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            string str2 = TicketSerializer.ReadWithDefault(reader, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
            int num = reader.ReadInt32();
            Claim[] claim = new Claim[num];
            for (int i = 0; i != num; i++)
            {
                string str3 = TicketSerializer.ReadWithDefault(reader, str1);
                string str4 = reader.ReadString();
                string str5 = TicketSerializer.ReadWithDefault(reader, "http://www.w3.org/2001/XMLSchema#string");
                string str6 = TicketSerializer.ReadWithDefault(reader, "LOCAL AUTHORITY");
                string str7 = TicketSerializer.ReadWithDefault(reader, str6);
                claim[i] = new Claim(str3, str4, str5, str6, str7);
            }
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claim, str, str1, str2);
            if (reader.ReadInt32() > 0)
            {
                claimsIdentity.BootstrapContext = new BootstrapContext(reader.ReadString()); //--> Contains: BootstrapContext(RSASecurityToken token, SecurityTokenHandler tokenHandler)
            }
            return new AuthenticationTicket(claimsIdentity, PropertiesSerializer.Read(reader)); //--> Properties contain the Expires, Issued values
        }

        private static string ReadWithDefault(BinaryReader reader, string defaultValue)
        {
            string str = reader.ReadString();
            if (string.Equals(str, "\0", StringComparison.Ordinal))
            {
                return defaultValue;
            }
            return str;
        }

        public virtual byte[] Serialize(AuthenticationTicket model)
        {
            byte[] array;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
                {
                    using (BinaryWriter binaryWriter = new BinaryWriter(gZipStream))
                    {
                        TicketSerializer.Write(binaryWriter, model);
                    }
                }
                array = memoryStream.ToArray();
            }
            return array;
        }

        public static void Write(BinaryWriter writer, AuthenticationTicket model)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            writer.Write(3);
            ClaimsIdentity identity = model.Identity;
            writer.Write(identity.AuthenticationType);
            TicketSerializer.WriteWithDefault(writer, identity.NameClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            TicketSerializer.WriteWithDefault(writer, identity.RoleClaimType, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
            writer.Write(identity.Claims.Count<Claim>());
            foreach (Claim claim in identity.Claims)
            {
                TicketSerializer.WriteWithDefault(writer, claim.Type, identity.NameClaimType);
                writer.Write(claim.Value);
                TicketSerializer.WriteWithDefault(writer, claim.ValueType, "http://www.w3.org/2001/XMLSchema#string");
                TicketSerializer.WriteWithDefault(writer, claim.Issuer, "LOCAL AUTHORITY");
                TicketSerializer.WriteWithDefault(writer, claim.OriginalIssuer, claim.Issuer);
            }
            BootstrapContext bootstrapContext = identity.BootstrapContext as BootstrapContext;
            if (bootstrapContext == null || string.IsNullOrWhiteSpace(bootstrapContext.Token))
            {
                writer.Write(0);
            }
            else
            {
                writer.Write(bootstrapContext.Token.Length);
                writer.Write(bootstrapContext.Token);
            }
            PropertiesSerializer.Write(writer, model.Properties);
        }

        private static void WriteWithDefault(BinaryWriter writer, string value, string defaultValue)
        {
            if (string.Equals(value, defaultValue, StringComparison.Ordinal))
            {
                writer.Write("\0");
                return;
            }
            writer.Write(value);
        }

        private static class DefaultValues
        {
            public const string DefaultStringPlaceholder = "\0";

            public const string NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

            public const string RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

            public const string LocalAuthority = "LOCAL AUTHORITY";

            public const string StringValueType = "http://www.w3.org/2001/XMLSchema#string";
        }
    }

    public interface IDataSerializer<TModel>
    {
        TModel Deserialize(byte[] data);

        byte[] Serialize(TModel model);
    }
}


