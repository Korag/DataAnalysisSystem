using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.DatasetDTO;
using DataAnalysisSystem.Services.DesignPatterns.StategyDesignPattern.FileObjectSerializer.Serializer;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StategyDesignPattern.FileObjectSerializer;
using System;
using System.Collections.Generic;

namespace DataAnalysisSystem.Services.DesignPatterns.StategyDesignPattern.FileObjectSerializer
{
    public class CsvSerializerStrategy : ISerializerStrategy
    {
        private CsvSerializer _serializer;

        public CsvSerializerStrategy()
        {
            _serializer = new CsvSerializer();
        }

        public string ConvertFromObjectToSpecificFile(ICollection<DatasetColumnAbstract> dataSet)
        {
            throw new NotImplementedException();
        }

        public ICollection<DatasetColumnAbstract> MapFileContentToObject(string filePath, DatasetAdditionalParametersViewModel parameters)
        {
            return _serializer.ReadCsvFileAndMapToObject(filePath, parameters.Delimiter);
        }
    }
}
