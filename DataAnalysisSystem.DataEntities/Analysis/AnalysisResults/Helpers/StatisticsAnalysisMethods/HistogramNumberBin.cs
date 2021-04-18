namespace DataAnalysisSystem.DataEntities
{
    public class HistogramNumberBin
    {
        public HistogramNumberBin()
        {

        }

        public double LowerBound { get; set; }
        public double UpperBound { get; set; }
        public double Width { get; set; }

        public double Value { get; set; }
    }
}
