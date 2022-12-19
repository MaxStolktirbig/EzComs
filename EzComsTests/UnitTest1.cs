using EzComs.Model.Conversion;
namespace EzComsTests
{
    public class UnitTest1
    {
        [Fact]
        public void ConversionTest()
        {
            string jsonString = "{\"key1\": \"..value1\", \"key2\": { \"subkey1\": \"somevalue\",\"subkey2\": \"..subvalue1\" }, \"key3\": [ \"..listvalue1\", \"..listvalue2\" ] }";

            dynamic deserializedJsonString = JsonConvert.DeserializeObject<dynamic>(jsonString);
            if (deserializedJsonString == null) { throw new ConversionException<string, dynamic>(); }
            JsonAdapter.GetPathsFromObject(deserializedJsonString);
        }
    }
}