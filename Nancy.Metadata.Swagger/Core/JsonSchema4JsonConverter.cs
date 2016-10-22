using System;
using Newtonsoft.Json;
using NJsonSchema;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Nancy.Metadata.Swagger.Core
{
    public class JsonSchema4JsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Dictionary<string, JsonSchema4>);
        }

        public override bool CanRead => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var schemas = value as Dictionary<string, JsonSchema4>;

            var dict = new Dictionary<string, JToken>();

            foreach(var schema in schemas)
            {
                if (dict.Keys.Contains(schema.Key))
                {
                    continue;
                }

                var jObject = JObject.Parse(schema.Value.ToJson());

                jObject.Remove(nameof(schema.Value.Definitions).ToLowerInvariant());

                dict.Add(schema.Key, jObject);

                if(schema.Value.Definitions.Count > 0)
                {
                    foreach(var def in schema.Value.Definitions.Values)
                    {
                        if (dict.Keys.Contains(def.TypeNameRaw))
                        {
                            continue;
                        }

                        dict.Add(def.TypeNameRaw, JToken.Parse(def.ToJson()));
                    }
                }
            }

            var json = JsonConvert.SerializeObject(dict, Formatting.None);

            writer.WriteRawValue(json);
        }
    }
}
