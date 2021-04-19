using DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails;

namespace DataAnalysisSystem.DTO.AnalysisResultsDTO
{
    public class AnalysisResultsDetailsViewModel
    {
        public DetailsKMeansClusteringResultViewModel KMeansClusteringResult { get; set; }
        public DetailsRegressionResultViewModel RegressionResult { get; set; }
        public DetailsApproximationResultViewModel2 ApproximationResult { get; set; }
        public DetailsDeriverativeResultViewModel DeriverativeResult { get; set; }
        public DetailsBasicStatisticsResultViewModel BasicStatisticsResult { get; set; }
        public DetailsHistogramResultViewModel HistogramResult { get; set; }
    }
}
