namespace DataAnalysisSystem.DataEntities
{
    public class DatasetContentSelectColumnForHistogramParametersTypeString
    {
        public DatasetContentSelectColumnForHistogramParametersTypeString()
        {

        }

        public DatasetContentSelectColumnForHistogramParametersTypeString(string attributeName, int positionInDataset)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }

        public bool ColumnSelected { get; set; }
    }
}
