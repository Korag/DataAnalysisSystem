using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class DatasetContentBasicStatisticsResultsTypeDouble
    {
        public DatasetContentBasicStatisticsResultsTypeDouble()
        {

        }

        public DatasetContentBasicStatisticsResultsTypeDouble(string attributeName, int positionInDataset, bool columnSelected)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;
            this.ColumnSelected = columnSelected;
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }

        public bool ColumnSelected { get; set; }

        public double Max { get; set; }
        public double Min { get; set; }
        public double Mean { get; set; }
        public double Median { get; set; }
        public double Variance { get; set; }
        public double StdDev { get; set; }
        public double Kurtosis { get; set; }
        public double Skewness { get; set; }
        public double Covariance { get; set; }
        public double LowerQuartile { get; set; }
        public double UpperQuartile { get; set; }
    }
}
