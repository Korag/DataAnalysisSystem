namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters
{
    public class DatasetContentSelectColumnForHistogramParametersTypeStringViewModel
    {
        public DatasetContentSelectColumnForHistogramParametersTypeStringViewModel()
        {

        }

        public DatasetContentSelectColumnForHistogramParametersTypeStringViewModel(string attributeName, int positionInDataset)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }

        public bool ColumnSelected { get; set; }
    }
}
