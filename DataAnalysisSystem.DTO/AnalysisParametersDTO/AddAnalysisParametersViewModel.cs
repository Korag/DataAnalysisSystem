using DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO
{
    public class AddAnalysisParametersViewModel
    {
        public AddKMeansClusteringParametersViewModel KMeansClusteringParameters { get; set; }
        public AddRegressionParametersViewModel RegressionParameters { get; set; }
        public AddApproximationParametersViewModel ApproximationParameters { get; set; }
        public AddDeriverativeParametersViewModel DeriverativeParameters { get; set; }
        public AddBasicStatisticsParametersViewModel BasicStatisticsParameters { get; set; }
        public AddHistogramParametersViewModel HistogramParameters { get; set; }
    }
}
