namespace Sunplus.Data.DataAbstract.Utils
{
    public static class JsonUtils
    {
        public static string SerializeObject(object data)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(data);
        }

        public static T DeserializeObject<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }
}
