using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAnalysisSystem.DataEntities
{
    [BsonIgnoreExtraElements]
    public class AnalysisResults
    {
        public AnalysisResults()
        {

        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AnalysisResultsIdentificator { get; set; }

        public KMeansClusteringResult KMeansClusteringResult { get; set; }
        public RegressionResult RegressionResult { get; set; }
        public ApproximationResult ApproximationResult { get; set; }
        public DeriverativeResult DeriverativeResult { get; set; }
        public BasicStatisticsResult BasicStatisticsResult { get; set; }
        public HistogramResult HistogramResult { get; set; }

    }
}
