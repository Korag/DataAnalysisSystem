using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class DatasetContentHistogramResultsTypeDouble
    {
        public DatasetContentHistogramResultsTypeDouble()
        {

        }

        public DatasetContentHistogramResultsTypeDouble(string attributeName, int positionInDataset, bool columnSelected)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;
            this.ColumnSelected = columnSelected;

            this.HistogramValues = new List<HistogramNumberBin>();
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }

        public bool ColumnSelected { get; set; }
        public int Range { get; set; }
        public IList<HistogramNumberBin> HistogramValues { get; set; }
    }
}
