using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.DatasetDTO;

namespace DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StrategyDesignPattern.FileObjectSerializer
{
    public interface ISerializerStrategy
    {
        public DatasetContent MapFileContentToObject(string pathToFile, DatasetAdditionalParametersViewModel parameters);
        public string ConvertFromObjectToSpecificFileFormatString(DatasetContent datasetContent);
    }
}
