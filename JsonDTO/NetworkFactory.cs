using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RadioactiveDecayLib;

namespace JsonDTO
{
    public static class NetworkFactory
    {
        public static string Serialize(object Object) => JsonConvert.SerializeObject(Object);

        public static T Deserialize<T>(string JsonObject) => JsonConvert.DeserializeObject<T>(JsonObject);

       
        public static object DeserializeAuto(string JsonObject)
        {
            JObject JO = JObject.Parse(JsonObject);
            string type = (string)JO["Type"];
            switch (type)
            {
                case nameof(Student): return Deserialize<Student>(JsonObject);
                case nameof(Element): return Deserialize<Element>(JsonObject);
                case nameof(Response): return Deserialize<Response>(JsonObject);
                case nameof(Result): return Deserialize<Result>(JsonObject);
                default: return null;
            }
        }
        
    }
}
