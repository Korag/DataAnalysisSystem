namespace DataAnalysisSystem.DTO.Helpers
{
    public class DatasetContentDeriverativeResultsTypeStringViewModel
    {
        public DatasetContentDeriverativeResultsTypeStringViewModel()
        {

        }

        public DatasetContentDeriverativeResultsTypeStringViewModel(string attributeName, int positionInDataset, bool columnSelected)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;
            this.ColumnSelected = columnSelected;
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }

        public bool ColumnSelected { get; set; }
    }
}
