using DataAnalysisSystem.DataEntities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DataAnalysisSystem.Services.DesignPatterns.StrategyDesignPattern.FileObjectSerializer.Serializer
{
    public class JsonSerializer
    {
        private const string REGEX_DOUBLE_PATTERN = @"^[0-9]*(?:\.[0-9]*)?$";
        private const string REGEX_INT_PATTERN = @"^\d$";

        public JsonSerializer()
        {
        }

        public DatasetContent ReadJsonFileAndMapToObject(string filePath)
        {
            string jsonFileContent = FileHelper.ReadFileContentToStringStatic(filePath);
            JArray jsonArrayObjectFileContent = new JArray();

            try
            {
                jsonArrayObjectFileContent = JArray.Parse(jsonFileContent);
            }
            catch (Exception e)
            {
                return null;
            }

            DatasetContent datasetContent = new DatasetContent();
            Regex regexDouble = new Regex(REGEX_DOUBLE_PATTERN);
            Regex regexInt = new Regex(REGEX_INT_PATTERN);

            JObject firstJsonObject = jsonArrayObjectFileContent.Children<JObject>().FirstOrDefault();

            int i = 0;
            foreach (JProperty property in firstJsonObject.Properties())
            {
                if (!String.IsNullOrWhiteSpace(Convert.ToString(property.Value)) && (regexDouble.IsMatch(Convert.ToString(property.Value)) || regexInt.IsMatch(Convert.ToString(property.Value))))
                {
                    DatasetColumnTypeDouble column = new DatasetColumnTypeDouble(Convert.ToString(property.Name), i);
                    datasetContent.NumberColumns.Add(column);
                }
                else
                {
                    DatasetColumnTypeString column = new DatasetColumnTypeString(Convert.ToString(property.Name), i);
                    datasetContent.StringColumns.Add(column);
                }

                i++;
            }

            foreach (var jsonObject in jsonArrayObjectFileContent.Children<JObject>())
            {
                int j = 0;

                foreach (JProperty property in jsonObject.Properties())
                {
                    if (datasetContent.NumberColumns.Where(z => z.PositionInDataset == j && z.AttributeName == Convert.ToString(property.Name))
                                                                .Count() != 0)
                    {
                        if (String.IsNullOrWhiteSpace(Convert.ToString(property.Value)))
                        {
                            return null;
                        }

                        datasetContent.NumberColumns.Where(z => z.PositionInDataset == j && z.AttributeName == Convert.ToString(property.Name))
                                                    .Select(z => z.AttributeValue)
                                                    .FirstOrDefault().Add(double.Parse(Convert.ToString(property.Value), CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        datasetContent.StringColumns.Where(z => z.PositionInDataset == j && z.AttributeName == Convert.ToString(property.Name))
                                                                                   .Select(z => z.AttributeValue)
                                                                                   .FirstOrDefault().Add(Convert.ToString(property.Value));
                    }
                    j++;
                }
            }

            return datasetContent;
        }

        public string ConvertFromObjectToJsonString(DatasetContent datasetContent)
        {
            List<dynamic> dynamicCollection = SerializerHelper.MapDatasetContentObjectToDynamicObject(datasetContent).ToList();
            var settings = new JsonSerializerSettings { Formatting = Formatting.Indented };

            string jsonString = JsonConvert.SerializeObject(dynamicCollection, settings);
            return jsonString;
        }

        public static dynamic ConvertJsonStringToDynamicObject(string jsonString)
        {
            return JsonConvert.DeserializeObject(jsonString);
        }
    }
}
