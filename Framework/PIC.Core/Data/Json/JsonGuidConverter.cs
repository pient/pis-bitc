using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PIC.Data
{
    public class JsonGuidConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
                writer.WriteValue(String.Empty);
            else
                writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null || reader.Value == null)
                return null;

            var value = reader.Value.ToString();

            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return Guid.Parse(value);
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Guid?) || objectType == typeof(Guid);
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }
    }
}
