using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class DatasetContentHistogramResultsTypeString
    {
        public DatasetContentHistogramResultsTypeString()
        {

        }

        public DatasetContentHistogramResultsTypeString(string attributeName, int positionInDataset, bool columnSelected)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;
            this.ColumnSelected = columnSelected;

            this.HistogramValues = new List<HistogramStringBin>();
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }

        public bool ColumnSelected { get; set; }
        public IList<HistogramStringBin> HistogramValues { get; set; }
    }
}
