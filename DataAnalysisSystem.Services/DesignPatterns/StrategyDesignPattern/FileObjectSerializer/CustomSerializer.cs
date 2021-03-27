using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.DatasetDTO;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StrategyDesignPattern.FileObjectSerializer;

namespace DataAnalysisSystem.Services.DesignPatterns.StrategyDesignPattern.FileObjectSerializer
{
    public sealed class CustomSerializer
    {
        private static ISerializerStrategy _serializerStrategy = null;

        public CustomSerializer()
        {

        }

        public CustomSerializer(ISerializerStrategy serializerStrategy)
        {
            _serializerStrategy = serializerStrategy;
        }

        public void ChangeStrategy(ISerializerStrategy serializerStrategy)
        {
            _serializerStrategy = serializerStrategy;
        }

        public DatasetContent MapFileContentToObject(string filePath, DatasetAdditionalParametersViewModel parameters)
        {
            return _serializerStrategy.MapFileContentToObject(filePath, parameters);
        }

        public string ConvertFromObjectToSpecificFile(DatasetContent datasetContent)
        {
            return _serializerStrategy.ConvertFromObjectToSpecificFileFormatString(datasetContent);
        }
    }
}