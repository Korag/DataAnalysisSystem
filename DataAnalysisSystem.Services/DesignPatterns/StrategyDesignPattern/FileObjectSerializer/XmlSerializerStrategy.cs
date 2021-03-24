using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DesignPatterns.StrategyDesignPattern.FileObjectSerializer.Serializer;
using DataAnalysisSystem.DTO.DatasetDTO;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StrategyDesignPattern.FileObjectSerializer;

namespace DataAnalysisSystem.Services.DesignPatterns.StrategyDesignPattern.FileObjectSerializer
{
    public class XmlSerializerStrategy : ISerializerStrategy
    {
        private XmlSerializer _serializer;

        public XmlSerializerStrategy()
        {
            _serializer = new XmlSerializer();
        }

        public string ConvertFromObjectToSpecificFile(DatasetContent datasetContent)
        {
            return "XML";
        }

        public DatasetContent MapFileContentToObject(string filePath, DatasetAdditionalParametersViewModel parameters)
        {
            return _serializer.ReadXmlFileAndMapToObject(filePath);
        }
    }
}
