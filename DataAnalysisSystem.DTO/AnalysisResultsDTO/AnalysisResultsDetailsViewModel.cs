using DataAnalysisSystem.DataEntities;

namespace DataAnalysisSystem.DTO.AnalysisResultsDTO
{
    public class AnalysisResultsDetailsViewModel
    {
        public KMeansClusteringResult KMeansClusteringResult { get; set; }
        public RegressionResult RegressionResult { get; set; }
        public ApproximationResult ApproximationResult { get; set; }
        public DeriverativeResult DeriverativeResult { get; set; }
        public BasicStatisticsResult BasicStatisticsResult { get; set; }
        public HistogramResult HistogramResult { get; set; }
    }
}
