using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class Dataset
    {
        public Dataset()
        {
            this.DatasetContent = new List<string>();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DatasetIdentificator { get; set; }
        public string Name { get; set; }

        public string DateOfCreation { get; set; }
        public string DateOfEdition { get; set; }

        public bool Shared { get; set; }
        public string AccessKey { get; set; }

        //Układ wierszy-kolumn zawierających dynamicznie przypisane dane
        //struktura musi być dynamiczna
        public ICollection<string> DatasetContent { get; set; }
        //Statystyki zbioru
        public DatasetStatistics DatasetStatistics { get; set; }
    }
}
