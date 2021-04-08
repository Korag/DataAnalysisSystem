using DataAnalysisSystem.DTO.AnalysisParametersDTO.ParametersDetails;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO
{
    public class AnalysisParametersDetailsViewModel
    {
        public KMeansClusteringParametersDetailsViewModel KMeansClusteringParameters { get; set; }
        public RegressionParametersDetailsViewModel RegressionParameters { get; set; }
        public ApproximationParametersDetailsViewModel ApproximationParameters { get; set; }
        public DeriverativeParametersDetailsViewModel DeriverativeParameters { get; set; }
        public BasicStatisticsParametersDetailsViewModel BasicStatisticsParameters { get; set; }
        public HistogramParametersDetailsViewModel HistogramParameters { get; set; }
    }
}
