using Newtonsoft.Json;

namespace DataAnalysisSystem.Services.DesignPatterns.StategyDesignPattern.FileObjectSerializer.Serializer
{
    public static class JsonSerializer
    {
        public static dynamic ConvertJsonStringToDynamicObject(string jsonString)
        {
            return JsonConvert.DeserializeObject(jsonString);
        }
    }
}
