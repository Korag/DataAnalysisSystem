﻿using DataAnalysisSystem.DataEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DataAnalysisSystem.Services.DesignPatterns.StrategyDesignPattern.FileObjectSerializer.Serializer
{
    public class XmlSerializer
    {
        private const string REGEX_DOUBLE_PATTERN = @"^[0-9]*(?:\.[0-9]*)?$";
        private const string REGEX_INT_PATTERN = @"^\d$";

        public XmlSerializer()
        {

        }

        public DatasetContent ReadXmlFileAndMapToObject(string filePath)
        {
            string xmlFileContent = FileHelper.ReadFileContentToStringStatic(filePath);

            XDocument xmlDocument = XDocument.Parse(xmlFileContent);
            string jsonFileContent = JsonConvert.SerializeXNode(xmlDocument.Root);
            dynamic dynamicXML = JsonConvert.DeserializeObject<ExpandoObject>(jsonFileContent);

            DatasetContent datasetContent = new DatasetContent();
            Regex regexDouble = new Regex(REGEX_DOUBLE_PATTERN);
            Regex regexInt = new Regex(REGEX_INT_PATTERN);

            foreach (var root in (IDictionary<String, Object>)dynamicXML)
            {
                foreach (var records in (IDictionary<String, Object>)root.Value)
                {
                    int i = 0;
                    foreach (var record in (List<Object>)records.Value)
                    {
                        int j = 0;
                        foreach (var attribute in (IDictionary<String, Object>)record)
                        {
                            if (i == 0)
                            {
                                if (!String.IsNullOrWhiteSpace(Convert.ToString(attribute.Value)) && (regexDouble.IsMatch(Convert.ToString(attribute.Value)) || regexInt.IsMatch(Convert.ToString(attribute.Value))))
                                {
                                    DatasetColumnTypeDouble column = new DatasetColumnTypeDouble(Convert.ToString(attribute.Key), j);
                                    column.AttributeValue.Add(double.Parse(Convert.ToString(attribute.Value), CultureInfo.InvariantCulture));

                                    datasetContent.NumberColumns.Add(column);
                                }
                                else
                                {
                                    DatasetColumnTypeString column = new DatasetColumnTypeString(Convert.ToString(attribute.Key), j);
                                    column.AttributeValue.Add(Convert.ToString(attribute.Value));

                                    datasetContent.StringColumns.Add(column);
                                }
                            }

                            else if (datasetContent.NumberColumns.Where(z => z.PositionInDataset == j && z.AttributeName == attribute.Key)
                                                                .Count() != 0)
                            {
                                if (String.IsNullOrWhiteSpace(Convert.ToString(attribute.Value)))
                                {
                                    return null;
                                }

                                datasetContent.NumberColumns.Where(z => z.PositionInDataset == j && z.AttributeName == attribute.Key)
                                                            .Select(z => z.AttributeValue)
                                                            .FirstOrDefault().Add(double.Parse(Convert.ToString(attribute.Value), CultureInfo.InvariantCulture));
                            }
                            else
                            {
                                datasetContent.StringColumns.Where(z => z.PositionInDataset == j && z.AttributeName == attribute.Key)
                                                                                           .Select(z => z.AttributeValue)
                                                                                           .FirstOrDefault().Add(Convert.ToString(attribute.Value));
                            }
                            j++;
                        }
                        i++;
                    }
                }
            }

            return datasetContent;
        }

        public string ConvertFromObjectToXmlString(DatasetContent datasetContent)
        {
            List<dynamic> dynamicCollection = SerializerHelper.MapDatasetContentObjectToDynamicObject(datasetContent).ToList();
            string result = "";

            //var dictionary = new Dictionary<string, object>(dynamicCollection.FirstOrDefault());
            //using (var writer = new StringWriter())
            //{
            //    Console.SetOut(writer);
            //    Console.SetError(writer);

            //    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(dictionary.GetType());
            //    x.Serialize(Console.Out, dictionary);


            //    result = writer.ToString();
            //}

            //string outputXml = "";
            //var list = dynamicCollection.Select(expando => (IDictionary<string, object>)expando).ToList();

            //var dataContractSerializer = new DataContractSerializer(list.GetType());
            //using (MemoryStream memoryStream = new MemoryStream())
            //{
            //    dataContractSerializer.WriteObject(memoryStream, list);
            //    outputXml = Encoding.UTF8.GetString(memoryStream.ToArray());
            //};

            return result;
        }
    }
}
