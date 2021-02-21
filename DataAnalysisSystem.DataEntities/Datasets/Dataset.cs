using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAnalysisSystem.DataEntities
{
    public class Dataset
    {
        public Dataset()
        {
            this.DatasetContent = new DatasetContent();
            this.DatasetStatistics = new DatasetStatistics();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DatasetIdentificator { get; set; }
        public string DatasetName { get; set; }

        public string DateOfCreation { get; set; }
        public string DateOfEdition { get; set; }

        public bool IsShared { get; set; }
        public string AccessKey { get; set; }

        public DatasetContent DatasetContent { get; set; }
        public DatasetStatistics DatasetStatistics { get; set; }
    }
}
