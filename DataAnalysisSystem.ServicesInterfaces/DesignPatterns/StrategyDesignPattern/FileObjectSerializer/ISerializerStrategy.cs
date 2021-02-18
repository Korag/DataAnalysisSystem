using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.DatasetDTO;
using System.Collections.Generic;

namespace DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StategyDesignPattern.FileObjectSerializer
{
    public interface ISerializerStrategy
    {
        public ICollection<DatasetColumnAbstract> MapFileContentToObject(string pathToFile, DatasetAdditionalParametersViewModel parameters);
        public string ConvertFromObjectToSpecificFile(ICollection<DatasetColumnAbstract> dataSet);
    }
}
