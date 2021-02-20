using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.DatasetDTO;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StategyDesignPattern.FileObjectSerializer;
using System.Collections.Generic;

namespace DataAnalysisSystem.Services.DesignPatterns.StategyDesignPattern.FileObjectSerializer
{
    public sealed class CustomSerializer
    {
        private static ISerializerStrategy _serializerStrategy = null;
        private static CustomSerializer serializer = null;

        private CustomSerializer(ISerializerStrategy serializerStrategy)
        {
            _serializerStrategy = serializerStrategy;
        }

        public static CustomSerializer GetInstance(ISerializerStrategy serializerStrategy)
        {
            if (serializer == null)
            {
                serializer = new CustomSerializer(serializerStrategy);
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

        public string ConvertFromObjectToSpecificFile(ICollection<DatasetColumnAbstract> dataSet)
        {
            return _serializerStrategy.ConvertFromObjectToSpecificFile(dataSet);
        }
    }
}