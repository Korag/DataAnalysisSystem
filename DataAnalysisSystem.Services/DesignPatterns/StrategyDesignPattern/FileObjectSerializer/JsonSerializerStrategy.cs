using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.DatasetDTO;
using DataAnalysisSystem.Services.DesignPatterns.StrategyDesignPattern.FileObjectSerializer.Serializer;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StrategyDesignPattern.FileObjectSerializer;

namespace DataAnalysisSystem.Services.DesignPatterns.StrategyDesignPattern.FileObjectSerializer
{
    public class JsonSerializerStrategy : ISerializerStrategy
    {
        private JsonSerializer _serializer;

        public JsonSerializerStrategy()
        {
            _serializer = new JsonSerializer();
        }

        public string ConvertFromObjectToSpecificFileFormatString(DatasetContent datasetContent)
        {
            return _serializer.ConvertFromObjectToJsonString(datasetContent);
        }

        public DatasetContent MapFileContentToObject(string filePath, DatasetAdditionalParametersViewModel parameters)
        {
            return _serializer.ReadJsonFileAndMapToObject(filePath);
        }
    }
}
