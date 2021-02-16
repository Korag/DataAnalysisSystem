using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.ServicesInterfaces.Serializers;
using System;
using System.Collections.Generic;

namespace DataAnalysisSystem.Services.Serializers
{
    public class XmlSerializerStrategy : ISerializerStrategy
    {
        public XmlSerializerStrategy()
        {

        }

        public string ConvertFromObjectToSpecificFile(ICollection<DatasetColumnAbstract> dataSet)
        {
            throw new NotImplementedException();
        }

        public ICollection<DatasetColumnAbstract> MapFileContentToObject(string fileContent)
        {
            throw new NotImplementedException();
        }
    }
}
