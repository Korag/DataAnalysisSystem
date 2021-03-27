using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.DatasetDTO;
using DataAnalysisSystem.Services.DesignPatterns.StrategyDesignPattern.FileObjectSerializer.Serializer;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StrategyDesignPattern.FileObjectSerializer;

namespace DataAnalysisSystem.Services.DesignPatterns.StrategyDesignPattern.FileObjectSerializer
{
    public class XlsxSerializerStrategy : ISerializerStrategy
    {
        private XlsxSerializer _serializer;

        public XlsxSerializerStrategy()
        {
            _serializer = new XlsxSerializer();
        }

        public string ConvertFromObjectToSpecificFileFormatString(DatasetContent datasetContent)
        {
            return "Xlsx string";
        }

        public DatasetContent MapFileContentToObject(string filePath, DatasetAdditionalParametersViewModel parameters)
        {
            return _serializer.ReadXlsxFileAndMapToObject(filePath);
        }
    }
}
