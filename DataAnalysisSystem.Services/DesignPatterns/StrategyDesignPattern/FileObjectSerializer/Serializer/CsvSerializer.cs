using CsvHelper;
using System.Globalization;
using System.IO;

namespace DataAnalysisSystem.Services.DesignPatterns.StategyDesignPattern.FileObjectSerializer.Serializer
{
    public class CsvSerializer
    {
        public void ReadCsvFileAndMapToDynamicObject(string fileContent, string delimiter)
        {
            using (var reader = new StringReader(fileContent))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.Delimiter = delimiter;

                    csv.Read();
                    csv.ReadHeader();
                    string[] headerRow = csv.Context.HeaderRecord;

                    var records = csv.GetRecords<dynamic>();
                }
            }
        }
    }
}
