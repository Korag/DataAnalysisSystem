﻿using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.DatasetDTO;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StategyDesignPattern.FileObjectSerializer;
using System.Collections.Generic;

namespace DataAnalysisSystem.Services.DesignPatterns.StategyDesignPattern.FileObjectSerializer
{
    public sealed class CustomSerializerSingleton
    {
        private static ISerializerStrategy _serializerStrategy = null;
        private static CustomSerializerSingleton serializer = null;

        private CustomSerializerSingleton(ISerializerStrategy serializerStrategy)
        {
            _serializerStrategy = serializerStrategy;
        }

        public static CustomSerializerSingleton GetInstance(ISerializerStrategy serializerStrategy)
        {
            if (serializer == null)
            {
                serializer = new CustomSerializerSingleton(serializerStrategy);
            }

            return serializer;
        }

        public void ChangeStrategy(ISerializerStrategy serializerStrategy)
        {
            _serializerStrategy = serializerStrategy;
        }

        public DatasetContent MapFileContentToObject(string filePath, DatasetAdditionalParametersViewModel parameters)
        {
            return _serializerStrategy.MapFileContentToObject(filePath, parameters);
        }

        public string ConvertFromObjectToSpecificFile(DatasetContent datasetContent)
        {
            return _serializerStrategy.ConvertFromObjectToSpecificFile(datasetContent);
        }
    }
}