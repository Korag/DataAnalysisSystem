using CsvHelper;
using DataAnalysisSystem.DataEntities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace DataAnalysisSystem.Services.DesignPatterns.StategyDesignPattern.FileObjectSerializer.Serializer
{
    public class CsvSerializer
    {
        public ICollection<DatasetColumnAbstract> ReadCsvFileAndMapToObject(string filePath, string delimiter)
        {
            using (var reader = new StreamReader(filePath))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.Delimiter = delimiter;

                    csv.Read();
                    csv.ReadHeader();
                    
                    var data = csv.GetRecords<dynamic>();

                    string[] headerRow = csv.Context.HeaderRecord;

                    //Add headerDelimiter everywhere!!!
                    string[] variableNames = headerRow[0].Split(delimiter);

                    List<DatasetColumnAbstract> datasetContent = new List<DatasetColumnAbstract>();

                    foreach (var variable in variableNames)
                    {
                        DatasetColumnDouble test = new DatasetColumnDouble("test");
                        test.AttributeValue.Add(3.00);
                        datasetContent.Add(test);
                        datasetContent[0].AttributeDataType = "2";
                        DatasetColumnDouble abc = (DatasetColumnDouble)datasetContent[0];
                        abc.AttributeDataType = "3";

                        object A = 3;
                        object B = "asdb";
                        List<object> objects = new List<object>();
                        objects.Add(A);
                        objects.Add(B);

                    }

                    foreach (var rows in (IDictionary<String, Object>)data)
                    {
                        string[] variableValues = rows.Key.Split(delimiter);

                        for (int i=0; i < variableValues.Length; i++)
                        {
                            switch (datasetContent[i].AttributeDataType)
                            {
                                case ("double"):
                                    //datasetContent[i].AttributeValue.Add(variableValues[i]);
                                        break;

                                case ("string"):
                                    //datasetContent[i].AttributeDataType == "double"

                                    break;

                                default:
                                    break;
                            }
                            
                        }

                        //Console.WriteLine(property.Key + ": " + property.Value);
                    }

                 

                    var a = 0;
                    var b = 1;

                    return datasetContent;
                }
            }
        }
    }
}
