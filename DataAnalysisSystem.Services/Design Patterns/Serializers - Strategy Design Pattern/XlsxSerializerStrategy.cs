using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.ServicesInterfaces.Serializers;
using System;
using System.Collections.Generic;

namespace DataAnalysisSystem.Services.Serializers
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

        public ICollection<DatasetColumnAbstract> MapFileContentToObject(string fileContent)
        {
            throw new NotImplementedException();
        }
    }
}
