using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearerOAuthTokenDeserializer
{
    public class PropertiesSerializer : IDataSerializer<AuthenticationProperties>
    {
        private const int FormatVersion = 1;

        public PropertiesSerializer()
        {
        }

        public AuthenticationProperties Deserialize(byte[] data)
        {
            AuthenticationProperties authenticationProperty;
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                {
                    authenticationProperty = PropertiesSerializer.Read(binaryReader);
                }
            }
            return authenticationProperty;
        }

        public static AuthenticationProperties Read(BinaryReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            if (reader.ReadInt32() != 1)
            {
                return null;
            }
            int num = reader.ReadInt32();
            Dictionary<string, string> strs = new Dictionary<string, string>(num);
            for (int i = 0; i != num; i++)
            {
                strs.Add(reader.ReadString(), reader.ReadString());
            }
            return new AuthenticationProperties(strs);
        }

        public byte[] Serialize(AuthenticationProperties model)
        {
            byte[] array;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    PropertiesSerializer.Write(binaryWriter, model);
                    binaryWriter.Flush();
                    array = memoryStream.ToArray();
                }
            }
            return array;
        }

        public static void Write(BinaryWriter writer, AuthenticationProperties properties)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }
            writer.Write(1);
            writer.Write(properties.Dictionary.Count);
            foreach (KeyValuePair<string, string> dictionary in properties.Dictionary)
            {
                writer.Write(dictionary.Key);
                writer.Write(dictionary.Value);
            }
        }
    }
}
