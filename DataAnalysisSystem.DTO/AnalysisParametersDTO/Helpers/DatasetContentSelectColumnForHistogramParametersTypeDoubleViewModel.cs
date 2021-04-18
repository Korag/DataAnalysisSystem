using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.Helpers
{
    public class DatasetContentSelectColumnForHistogramParametersTypeDoubleViewModel
    {
        public DatasetContentSelectColumnForHistogramParametersTypeDoubleViewModel()
        {

        }

        public DatasetContentSelectColumnForHistogramParametersTypeDoubleViewModel(string attributeName, int positionInDataset, bool columnSelected = false)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;
            this.ColumnSelected = columnSelected;
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }

        public bool ColumnSelected { get; set; }

        [Range(typeof(int), "0", "2147483647")]
        public int Range { get; set; }
    }
}
