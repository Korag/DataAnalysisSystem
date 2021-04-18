namespace DataAnalysisSystem.DTO.Helpers
{
    public class HistogramNumberBinViewModel
    {
        public HistogramNumberBinViewModel()
        {

        }

        public double LowerBound { get; set; }
        public double UpperBound { get; set; }
        public double Width { get; set; }

        public double Value { get; set; }
    }
}
