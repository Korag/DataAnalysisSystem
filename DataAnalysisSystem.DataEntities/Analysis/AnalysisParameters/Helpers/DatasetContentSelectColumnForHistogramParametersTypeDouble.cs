namespace DataAnalysisSystem.DataEntities
{
    public class DatasetContentSelectColumnForHistogramParametersTypeDouble
    {
        public DatasetContentSelectColumnForHistogramParametersTypeDouble()
        {

        }

        public DatasetContentSelectColumnForHistogramParametersTypeDouble(string attributeName, int positionInDataset)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }

        public bool ColumnSelected { get; set; }
        public int Range { get; set; }
    }
}
