using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails;

namespace DataAnalysisSystem.DTO.AnalysisResultsDTO
{
    public class AnalysisResultsDetailsViewModel
    {
        public AnalysisResultsDetailsViewModel(AnalysisResults analysisResults)
        {
            this.ApproximationResult = new DetailsApproximationResultViewModel(analysisResults);
            this.DeriverativeResult = new DetailsDeriverativeResultViewModel(analysisResults);
            this.KMeansClusteringResult = new DetailsKMeansClusteringResultViewModel(analysisResults);
            this.RegressionResult = new DetailsRegressionResultViewModel(analysisResults);
            this.BasicStatisticsResult = new DetailsBasicStatisticsResultViewModel(analysisResults);
            this.HistogramResult = new DetailsHistogramResultViewModel(analysisResults);
        }

        public DetailsKMeansClusteringResultViewModel KMeansClusteringResult { get; set; }
        public DetailsRegressionResultViewModel RegressionResult { get; set; }
        public DetailsApproximationResultViewModel ApproximationResult { get; set; }
        public DetailsDeriverativeResultViewModel DeriverativeResult { get; set; }
        public DetailsBasicStatisticsResultViewModel BasicStatisticsResult { get; set; }
        public DetailsHistogramResultViewModel HistogramResult { get; set; }
    }
}
