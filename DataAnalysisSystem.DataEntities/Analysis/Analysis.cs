using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

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

        public string AnalysisIndexer { get; set; }
        public string DatasetIdentificator { get; set; }

        public string DateOfCreation { get; set; }

        public bool IsShared { get; set; }
        public string AccessKey { get; set; }

        public IList<string> PerformedAnalysisTypes { get; set; }
        public AnalysisParameters AnalysisParameters { get; set; }
        public AnalysisResults AnalysisResults { get; set; }
    }
}
