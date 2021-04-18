namespace DataAnalysisSystem.DTO.Helpers
{
    public class DatasetContentApproximationResultsTypeStringViewModel
    {
        public DatasetContentApproximationResultsTypeStringViewModel()
        {

        }

        public DatasetContentApproximationResultsTypeStringViewModel(string attributeName, int positionInDataset, bool columnSelected)
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
