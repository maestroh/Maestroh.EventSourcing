using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Maestroh.DataAccess.Tests
{
    public class Formatter
    {
        private static readonly BinaryFormatter _formatter;

        static Formatter()
        {
            _formatter = new BinaryFormatter();
        }

        public static byte[] Serialize(object theObject)
        {
            using (var memoryStream = new MemoryStream())
            {
                _formatter.Serialize(memoryStream, theObject);
                return memoryStream.ToArray();
            }
        }

        public static TType Deserialize<TType>(object bytes)
        {
            using (var memoryStream = new MemoryStream((Byte[])bytes))
            {
                return (TType)_formatter.Deserialize(memoryStream);
            }
        }
    }
}