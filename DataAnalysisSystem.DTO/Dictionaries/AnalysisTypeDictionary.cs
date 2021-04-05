using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.Dictionaries
{
    public static class AnalysisTypeDictionary
    {
         public static readonly Dictionary<string, string> AnalysisType = new Dictionary<string, string>
         {
           {"KMeansClusteringMethod", "K-Means Clustering"},
           {"RegressionMethod", "Linear Regression"},
           {"ApproximationMethod", "..."},
           {"DerivativeMethod", "..."},
           {"BasicStatisticsMethod", "Basic Set of Statistics Indicators"},
           {"HistogramMethod", "Histogram"},
        };
    }
}