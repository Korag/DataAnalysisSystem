using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.Helpers
{
    public class DatasetContentHistogramResultsTypeStringViewModel
    {
        public DatasetContentHistogramResultsTypeStringViewModel()
        {

        }

        public DatasetContentHistogramResultsTypeStringViewModel(string attributeName, int positionInDataset, bool columnSelected)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;
            this.ColumnSelected = columnSelected;

            this.HistogramValues = new List<HistogramStringBinViewModel>();
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }

        public bool ColumnSelected { get; set; }
        public IList<HistogramStringBinViewModel> HistogramValues { get; set; }
    }
}
