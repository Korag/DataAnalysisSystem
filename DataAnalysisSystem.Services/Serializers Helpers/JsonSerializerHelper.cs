using Newtonsoft.Json;

namespace DataAnalysisSystem.Services.Serializers_Helpers
{
    public static class JsonSerializerHelper
    {
        public static dynamic ConvertJsonStringToDynamicObject(string jsonString)
        {
            return JsonConvert.DeserializeObject(jsonString);
        }
    }
}
