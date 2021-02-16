using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.ServicesInterfaces.Serializers;
using System.Collections.Generic;

namespace DataAnalysisSystem.Services.Serializers
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

        public ICollection<DatasetColumnAbstract> MapFileContentToObject(string fileContent)
        {
            return _serializerStrategy.MapFileContentToObject(fileContent);
        }

        public string ConvertFromObjectToSpecificFile(ICollection<DatasetColumnAbstract> dataSet)
        {
            return _serializerStrategy.ConvertFromObjectToSpecificFile(dataSet);
        }
    }
}