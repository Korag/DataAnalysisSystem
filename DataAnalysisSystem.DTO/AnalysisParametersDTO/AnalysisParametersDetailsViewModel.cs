using DataAnalysisSystem.DTO.AnalysisParametersDTO.ParametersDetails;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO
{
    public class AnalysisParametersDetailsViewModel
    {
        public DetailsKMeansClusteringParametersViewModel KMeansClusteringParameters { get; set; }
        public DetailsRegressionParametersViewModel RegressionParameters { get; set; }
        public DetailsApproximationParametersViewModel ApproximationParameters { get; set; }
        public DetailsDeriverativeParametersViewModel DeriverativeParameters { get; set; }
        public DetailsBasicStatisticsParametersViewModel BasicStatisticsParameters { get; set; }
        public DetailsHistogramParametersViewModel HistogramParameters { get; set; }
    }
}
