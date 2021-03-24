using DataAnalysisSystem.DataEntities;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace DataAnalysisSystem.Services.DesignPatterns.StrategyDesignPattern.FileObjectSerializer.Serializer
{
    public static class SerializerHelper
    {
        public static IList<dynamic> MapDatasetContentObjectToDynamicObject(DatasetContent datasetContent)
        {
            int rowsAmount = datasetContent.NumberColumns.Count != 0
                                            ? datasetContent.NumberColumns.FirstOrDefault().AttributeValue.Count
                                            : datasetContent.StringColumns.FirstOrDefault().AttributeValue.Count;
            int attributesAmount = datasetContent.NumberColumns.Count + datasetContent.StringColumns.Count;

            dynamic[] dynamicCollection = new dynamic[rowsAmount];

            for (int i = 0; i < rowsAmount; i++)
            {
                dynamic singleRow = new ExpandoObject();
                IDictionary<string, object> dictionaryProperties = singleRow;

                for (int j = 0; j < attributesAmount; j++)
                {
                    DatasetColumnTypeDouble numberAttribute = datasetContent.NumberColumns.Where(z => z.PositionInDataset == j).FirstOrDefault();

                    if (numberAttribute != null)
                    {
                        dictionaryProperties.Add(numberAttribute.AttributeName, numberAttribute.AttributeValue[i].ToString());
                    }
                    else
                    {
                        DatasetColumnTypeString stringAttribute = datasetContent.StringColumns.Where(z => z.PositionInDataset == j).FirstOrDefault();

                        dictionaryProperties.Add(stringAttribute.AttributeName, stringAttribute.AttributeValue[i]);
                    }
                }

                dynamicCollection[i] = singleRow;
            }

            return dynamicCollection;
        }
    }
}
