using EzComs.Common.CustomExceptions;
using Newtonsoft.Json;

namespace EzComs.Model.ConversionContext
{
    public class JsonAdapter
    {
        public JsonAdapter() { }

        public string Convert(string source, string from, string to) {
            dynamic? deserializedJsonString = JsonConvert.DeserializeObject<dynamic>(source);
            if(deserializedJsonString == null) { throw new ConversionException<string, dynamic>(); }
            
            

        }

        private dynamic GetPathValue(dynamic source, string path)
        {

            if(path.Contains("."))
            {
                return GetPathValue(source, key);
            }
            return source[key];
        }
    }
}
