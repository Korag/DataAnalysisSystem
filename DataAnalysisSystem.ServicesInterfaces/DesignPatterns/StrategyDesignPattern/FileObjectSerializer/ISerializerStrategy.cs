using DataAnalysisSystem.DataEntities;
using System.Collections.Generic;

namespace DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StategyDesignPattern.FileObjectSerializer
{
    public interface ISerializerStrategy
    {
        public ICollection<DatasetColumnAbstract> MapFileContentToObject(string fileContent);
        public string ConvertFromObjectToSpecificFile(ICollection<DatasetColumnAbstract> dataSet);
    }
}
