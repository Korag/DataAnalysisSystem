using CsvHelper;
using DataAnalysisSystem.DataEntities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataAnalysisSystem.Services.DesignPatterns.StategyDesignPattern.FileObjectSerializer.Serializer
{
    public class CsvSerializer
    {
        private const string REGEX_DOUBLE_PATTERN = @"^[0-9]*(?:\.[0-9]*)?$";
        private const string REGEX_DOUBLE_PATTERN2 = @"(/\d+\.\d*|\.?\d+/)";
        private const string REGEX_INT_PATTERN = @"^\d$";

        public DatasetContent ReadCsvFileAndMapToObject(string filePath, string delimiter = ",")
        {
            using (var reader = new StreamReader(filePath))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.Delimiter = delimiter;

                    csv.Read();
                    csv.ReadHeader();

                    var records = csv.GetRecords<dynamic>();
                    string[] variableNames = csv.Context.HeaderRecord;

                    var firstRecord = records.FirstOrDefault();
                    List<string> firstRecordValues = new List<string>();

                    foreach (var attribute in (IDictionary<String, Object>)firstRecord)
                    {
                        firstRecordValues.Add(Convert.ToString(attribute.Value));
                    }

                    DatasetContent datasetContent = new DatasetContent();
                    Regex regexDouble = new Regex(REGEX_DOUBLE_PATTERN);
                    Regex regexInt = new Regex(REGEX_INT_PATTERN);

                    for (int i = 0; i < variableNames.Length; i++)
                    {
                        if (!String.IsNullOrWhiteSpace(firstRecordValues[i]) && (regexDouble.IsMatch(firstRecordValues[i]) || regexInt.IsMatch(firstRecordValues[i])))
                        {
                            DatasetColumnTypeDouble column = new DatasetColumnTypeDouble(variableNames[i], i);
                            column.AttributeValue.Add(double.Parse(Convert.ToString(firstRecordValues[i]), CultureInfo.InvariantCulture));

                            datasetContent.NumberColumns.Add(column);
                        }
                        else
                        {
                            DatasetColumnTypeString column = new DatasetColumnTypeString(variableNames[i], i);
                            column.AttributeValue.Add(Convert.ToString(firstRecordValues[i]));

                            datasetContent.StringColumns.Add(column);
                        }
                    }

                    foreach (var record in records)
                    {
                        int i = 0;

                        foreach (var attribute in (IDictionary<String, Object>)record)
                        {
                            if (datasetContent.NumberColumns.Where(z => z.PositionInDataset == i && z.AttributeName == attribute.Key)
                                                                .Count() != 0)
                            {
                                if (String.IsNullOrWhiteSpace(Convert.ToString(attribute.Value)))
                                {
                                    return null;
                                }

                                datasetContent.NumberColumns.Where(z => z.PositionInDataset == i && z.AttributeName == attribute.Key)
                                                            .Select(z => z.AttributeValue)
                                                            .FirstOrDefault().Add(double.Parse(Convert.ToString(attribute.Value), CultureInfo.InvariantCulture));
                            }
                            else
                            {
                                datasetContent.StringColumns.Where(z => z.PositionInDataset == i && z.AttributeName == attribute.Key)
                                                                                           .Select(z => z.AttributeValue)
                                                                                           .FirstOrDefault().Add(Convert.ToString(attribute.Value));
                            }
                            i++;
                        }
                    }

                    return datasetContent;
                }
            }
        }
    }
}
