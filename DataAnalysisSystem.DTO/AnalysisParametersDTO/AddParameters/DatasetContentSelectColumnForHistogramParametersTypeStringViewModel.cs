namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters
{
    public class DatasetContentSelectColumnForHistogramParametersTypeStringViewModel
    {
        public DatasetContentSelectColumnForHistogramParametersTypeStringViewModel()
        {

        }

        public DatasetContentSelectColumnForHistogramParametersTypeStringViewModel(string attributeName, int positionInDataset, bool columnSelected = false)
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
