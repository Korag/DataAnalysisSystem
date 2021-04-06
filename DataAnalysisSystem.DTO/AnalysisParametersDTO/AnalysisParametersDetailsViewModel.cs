using DataAnalysisSystem.DataEntities;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO
{
    public class AnalysisParametersDetailsViewModel
    {
        public KMeansClusteringParameters KMeansClusteringParameters { get; set; }
        public RegressionParameters RegressionParameters { get; set; }
        public ApproximationParameters ApproximationParameters { get; set; }
        public DeriverativeParameters DeriverativeParameters { get; set; }
        public BasicStatisticsParameters BasicStatisticsParameters { get; set; }
        public HistogramParameters HistogramParameters { get; set; }

    }
}
