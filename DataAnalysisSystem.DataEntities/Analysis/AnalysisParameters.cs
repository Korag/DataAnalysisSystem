﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAnalysisSystem.DataEntities
{
    [BsonIgnoreExtraElements]
    public class AnalysisParameters
    {
        public AnalysisParameters()
        {

        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AnalysisParametersIdentificator { get; set; }

        public KMeansClusteringParameters KMeansClusteringParameters { get; set; }
        public RegressionParameters RegressionParameters { get; set; }
        public ApproximationParameters ApproximationParameters { get; set; }
        public DeriverativeParameters DeriverativeParameters { get; set; }
        public BasicStatisticsParameters BasicStatisticsParameters { get; set; }
        public HistogramParameters HistogramParameters { get; set; }

    }
}
