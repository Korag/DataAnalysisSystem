using DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails;

namespace DataAnalysisSystem.DTO.AnalysisResultsDTO
{
    public class AnalysisResultsDetailsViewModel
    {
        public KMeansClusteringResultDetailsViewModel KMeansClusteringResult { get; set; }
        public RegressionResultDetailsViewModel RegressionResult { get; set; }
        public ApproximationResultDetailsViewModel ApproximationResult { get; set; }
        public DeriverativeResultDetailsViewModel DeriverativeResult { get; set; }
        public BasicStatisticsResultDetailsViewModel BasicStatisticsResult { get; set; }
        public HistogramResultDetailsViewModel HistogramResult { get; set; }
    }
}
