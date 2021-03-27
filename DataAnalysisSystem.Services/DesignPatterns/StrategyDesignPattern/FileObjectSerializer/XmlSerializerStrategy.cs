using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.DatasetDTO;
using DataAnalysisSystem.Services.DesignPatterns.StrategyDesignPattern.FileObjectSerializer.Serializer;
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

        public string ConvertFromObjectToSpecificFileFormatString(DatasetContent datasetContent)
        {
            return _serializer.ConvertFromObjectToXmlString(datasetContent);
        }

        public DatasetContent MapFileContentToObject(string filePath, DatasetAdditionalParametersViewModel parameters)
        {
            return _serializer.ReadXmlFileAndMapToObject(filePath);
        }
    }
}
