using DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters;
using DataAnalysisSystem.DTO.DatasetDTO;

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

        public AddAnalysisParametersViewModel(DatasetContentViewModel datasetContent)
        {
            this.KMeansClusteringParameters = new AddKMeansClusteringParametersViewModel(datasetContent);
            this.RegressionParameters = new AddRegressionParametersViewModel(datasetContent);
            this.ApproximationParameters = new AddApproximationParametersViewModel(datasetContent);
            this.DeriverativeParameters = new AddDeriverativeParametersViewModel(datasetContent);
            this.BasicStatisticsParameters = new AddBasicStatisticsParametersViewModel(datasetContent);
            this.HistogramParameters = new AddHistogramParametersViewModel(datasetContent);
        }

        public AddKMeansClusteringParametersViewModel KMeansClusteringParameters { get; set; }
        public AddRegressionParametersViewModel RegressionParameters { get; set; }
        public AddApproximationParametersViewModel ApproximationParameters { get; set; }
        public AddDeriverativeParametersViewModel DeriverativeParameters { get; set; }
        public AddBasicStatisticsParametersViewModel BasicStatisticsParameters { get; set; }
        public AddHistogramParametersViewModel HistogramParameters { get; set; }
    }
}
