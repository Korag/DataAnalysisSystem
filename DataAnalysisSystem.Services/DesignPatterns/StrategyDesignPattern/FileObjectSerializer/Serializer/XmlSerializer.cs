using DataAnalysisSystem.DataEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataAnalysisSystem.DesignPatterns.StategyDesignPattern.FileObjectSerializer.Serializer
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
                


            return new DatasetContent();
        }
    }
}
