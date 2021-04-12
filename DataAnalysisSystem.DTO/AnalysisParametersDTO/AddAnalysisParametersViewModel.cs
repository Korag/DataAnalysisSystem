using DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO
{
    public class AddAnalysisParametersViewModel
    {
        public AddAnalysisParametersViewModel()
        {
            this.KMeansClusteringParameters = new AddKMeansClusteringParametersViewModel();
            this.RegressionParameters = new AddRegressionParametersViewModel();
            this.ApproximationParameters = new AddApproximationParametersViewModel();
            this.DeriverativeParameters = new AddDeriverativeParametersViewModel();
            this.BasicStatisticsParameters = new AddBasicStatisticsParametersViewModel();
            this.HistogramParameters = new AddHistogramParametersViewModel();
        }

        public AddKMeansClusteringParametersViewModel KMeansClusteringParameters { get; set; }
        public AddRegressionParametersViewModel RegressionParameters { get; set; }
        public AddApproximationParametersViewModel ApproximationParameters { get; set; }
        public AddDeriverativeParametersViewModel DeriverativeParameters { get; set; }
        public AddBasicStatisticsParametersViewModel BasicStatisticsParameters { get; set; }
        public AddHistogramParametersViewModel HistogramParameters { get; set; }
    }
}
