using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAnalysisSystem.DataEntities
{
    public class Analysis
    {
        public Analysis()
        {

        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AnalysisIdentificator { get; set; }
        public string DatasetIdentificator { get; set; }

        public string DateOfCreation { get; set; }

        public bool IsShared { get; set; }
        public string AccessKey { get; set; }

        //Podsumowanie danych -> podstawowe informacje z dziedziny statystyki
        public DatasetSummary DatasetSummary { get; set; }
        //Wyniki wybranych metod, które będą posiadały wspólny interfejs
        public ICollection<string> Results { get; set; }
    }
}
