using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.DatasetDTO;
using DataAnalysisSystem.Services.DesignPatterns.StategyDesignPattern.FileObjectSerializer.Serializer;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StategyDesignPattern.FileObjectSerializer;

namespace DataAnalysisSystem.Services.DesignPatterns.StategyDesignPattern.FileObjectSerializer
{
    public class JsonSerializerStrategy : ISerializerStrategy
    {
        private JsonSerializer _serializer;

        public JsonSerializerStrategy()
        {
            _serializer = new JsonSerializer();
        }

        public string ConvertFromObjectToSpecificFile(DatasetContent datasetContent)
        {
            return _serializer.ConvertFromObjectToJsonString(datasetContent);
        }

        public DatasetContent MapFileContentToObject(string filePath, DatasetAdditionalParametersViewModel parameters)
        {
            return _serializer.ReadJsonFileAndMapToObject(filePath);
        }
    }
}
