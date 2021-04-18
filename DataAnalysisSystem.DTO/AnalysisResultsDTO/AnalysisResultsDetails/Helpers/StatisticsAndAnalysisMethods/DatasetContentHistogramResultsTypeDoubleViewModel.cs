using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.Helpers
{
    public class DatasetContentHistogramResultsTypeDoubleViewModel
    {
        public DatasetContentHistogramResultsTypeDoubleViewModel()
        {

        }

        public DatasetContentHistogramResultsTypeDoubleViewModel(string attributeName, int positionInDataset, bool columnSelected)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;
            this.ColumnSelected = columnSelected;

            this.HistogramValues = new List<HistogramNumberBinViewModel>();
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }

        public bool ColumnSelected { get; set; }
        public int Range { get; set; }
        public IList<HistogramNumberBinViewModel> HistogramValues { get; set; }
    }
}
