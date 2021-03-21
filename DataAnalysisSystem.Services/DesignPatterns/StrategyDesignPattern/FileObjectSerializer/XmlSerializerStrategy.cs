using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DesignPatterns.StategyDesignPattern.FileObjectSerializer.Serializer;
using DataAnalysisSystem.DTO.DatasetDTO;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StategyDesignPattern.FileObjectSerializer;
using System;
using System.Collections.Generic;

namespace DataAnalysisSystem.Services.DesignPatterns.StategyDesignPattern.FileObjectSerializer
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
            throw new NotImplementedException();
        }

        public DatasetContent MapFileContentToObject(string filePath, DatasetAdditionalParametersViewModel parameters)
        {
            return _serializer.ReadXmlFileAndMapToObject(filePath);
        }
    }
}
