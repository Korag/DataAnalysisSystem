using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.DatasetDTO;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StategyDesignPattern.FileObjectSerializer;
using System;
using System.Collections.Generic;

namespace DataAnalysisSystem.Services.DesignPatterns.StategyDesignPattern.FileObjectSerializer
{
    class XlsxSerializerStrategy : ISerializerStrategy
    {
        public XlsxSerializerStrategy()
        {

        }

        public string ConvertFromObjectToSpecificFile(ICollection<DatasetColumnAbstract> dataSet)
        {
            throw new NotImplementedException();
        }

        public DatasetContent MapFileContentToObject(string filePath, DatasetAdditionalParametersViewModel parameters)
        {
            throw new NotImplementedException();
        }
    }
}
