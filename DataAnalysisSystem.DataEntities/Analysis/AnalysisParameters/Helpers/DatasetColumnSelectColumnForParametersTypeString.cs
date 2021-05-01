namespace DataAnalysisSystem.DataEntities
{
    public class DatasetColumnSelectColumnForParametersTypeString
    {
        public DatasetColumnSelectColumnForParametersTypeString()
        {

        }

        public DatasetColumnSelectColumnForParametersTypeString(string attributeName, int positionInDataset, bool columnsSelected = false)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;
            this.ColumnSelected = columnsSelected;
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }

        public bool ColumnSelected { get; set; }
    }
}
