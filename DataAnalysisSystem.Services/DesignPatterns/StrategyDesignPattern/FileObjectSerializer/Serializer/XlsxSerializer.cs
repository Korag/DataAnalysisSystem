using DataAnalysisSystem.DataEntities;
using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataAnalysisSystem.Services.DesignPatterns.StrategyDesignPattern.FileObjectSerializer.Serializer
{
    public class XlsxSerializer
    {
        private const string REGEX_DOUBLE_PATTERN = @"^[0-9]*(?:\.[0-9]*)?$";
        private const string REGEX_INT_PATTERN = @"^\d$";
        private const string XLSX_SHEET_NAME = @"Dataset";

        public XlsxSerializer()
        {
        }

        public DatasetContent ReadXlsxFileAndMapToObject(string filePath)
        {
            IEnumerable<dynamic> records;

            try
            {
                records = new ExcelMapper(filePath).Fetch();
            }
            catch (Exception e)
            {
                return null;
            }

            DatasetContent datasetContent = new DatasetContent();
            Regex regexDouble = new Regex(REGEX_DOUBLE_PATTERN);
            Regex regexInt = new Regex(REGEX_INT_PATTERN);

            int i = 0;
            int positionInDataset = 0;
            foreach (var attribute in (IDictionary<String, Object>)records.FirstOrDefault())
            {
                if (i % 2 == 0)
                {
                    positionInDataset = Convert.ToInt32(attribute.Key);
                    i++;
                    continue;
                }

                if (!String.IsNullOrWhiteSpace(attribute.Key) && (regexDouble.IsMatch(Convert.ToString(attribute.Value)) || regexInt.IsMatch(Convert.ToString(attribute.Value))))
                {
                    DatasetColumnTypeDouble column = new DatasetColumnTypeDouble(attribute.Key, positionInDataset);
                    datasetContent.NumberColumns.Add(column);
                }
                else
                {
                    DatasetColumnTypeString column = new DatasetColumnTypeString(attribute.Key, positionInDataset);
                    datasetContent.StringColumns.Add(column);
                }
                i++;
            }

            positionInDataset = 0;
            foreach (var record in records)
            {
                int j = 0;

                foreach (var attribute in (IDictionary<String, Object>)record)
                {
                    if (j % 2 == 0)
                    {
                        positionInDataset = Convert.ToInt32(attribute.Key);
                        j++;
                        continue;
                    }

                    if (datasetContent.NumberColumns.Where(z => z.PositionInDataset == positionInDataset && z.AttributeName == attribute.Key)
                                                        .Count() != 0)
                    {
                        if (String.IsNullOrWhiteSpace(Convert.ToString(attribute.Value)))
                        {
                            return null;
                        }

                        datasetContent.NumberColumns.Where(z => z.PositionInDataset == positionInDataset && z.AttributeName == attribute.Key)
                                                    .Select(z => z.AttributeValue)
                                                    .FirstOrDefault().Add(double.Parse(Convert.ToString(attribute.Value), CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        datasetContent.StringColumns.Where(z => z.PositionInDataset == positionInDataset && z.AttributeName == attribute.Key)
                                                                                   .Select(z => z.AttributeValue)
                                                                                   .FirstOrDefault().Add(Convert.ToString(attribute.Value));
                    }
                    j++;
                }
            }

            return datasetContent;
        }

        public void SaveXlsxFileWithDatasetContent(DatasetContent datasetContent, string fullPath)
        {
            List<dynamic> dynamicCollection = SerializerHelper.MapDatasetContentObjectToDynamicObject(datasetContent).ToList();

            new ExcelMapper().Save(fullPath, dynamicCollection, XLSX_SHEET_NAME);
        }
    }
}
