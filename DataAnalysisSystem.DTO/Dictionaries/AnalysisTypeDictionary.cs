using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.Dictionaries
{
    public static class AnalysisTypeDictionary
    {
         public static readonly Dictionary<string, string> AnalysisType = new Dictionary<string, string>
         {
           {"approximationMethod", "K-Means Clustering"},
           {"basicStatisticsMethod", "Basic Set of Statistics Indicators"},
           {"deriverativeMethod", "Deriverative"},
           {"histogramMethod", "Histogram"},
           {"kMeansClusteringMethod", "K-Means Clustering"},
           {"regressionMethod", "Linear Regression"},
        };
    }
}