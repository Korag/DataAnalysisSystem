using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.DatasetDTO;
using DataAnalysisSystem.Services.DesignPatterns.StategyDesignPattern.FileObjectSerializer.Serializer;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StategyDesignPattern.FileObjectSerializer;
using System;
using System.Collections.Generic;

namespace DataAnalysisSystem.Services.DesignPatterns.StategyDesignPattern.FileObjectSerializer
{
    public class XlsxSerializerStrategy : ISerializerStrategy
    {
        private XlsxSerializer _serializer;

        public XlsxSerializerStrategy()
        {
            _serializer = new XlsxSerializer();
        }

        public string ConvertFromObjectToSpecificFile(DatasetContent datasetContent)
        {
            return "XLSX";
        }

        public DatasetContent MapFileContentToObject(string filePath, DatasetAdditionalParametersViewModel parameters)
        {
            return _serializer.ReadXlsxFileAndMapToObject(filePath);
        }
    }
}
