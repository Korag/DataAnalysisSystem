﻿using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.DatasetDTO;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StategyDesignPattern.FileObjectSerializer;
using System;
using System.Collections.Generic;

namespace DataAnalysisSystem.Services.DesignPatterns.StategyDesignPattern.FileObjectSerializer
{
    public class JsonSerializerStrategy : ISerializerStrategy
    {
        public JsonSerializerStrategy()
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
