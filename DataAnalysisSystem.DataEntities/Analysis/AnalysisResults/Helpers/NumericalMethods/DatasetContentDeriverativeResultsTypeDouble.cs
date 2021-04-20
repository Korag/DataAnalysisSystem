using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class DatasetContentDeriverativeResultsTypeDouble
    {
        public DatasetContentDeriverativeResultsTypeDouble()
        {

        }

        public DatasetContentDeriverativeResultsTypeDouble(string attributeName, int positionInDataset, bool columnSelected)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;
            this.ColumnSelected = columnSelected;

            this.InX = new List<double>();
            this.InY = new List<double>();

            this.OutX = new List<double>();
            this.OutY = new List<double>();

            this.DYDX = new List<double>();
            this.DY2DX2 = new List<double>();
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }

        public bool ColumnSelected { get; set; }

        public IList<double> InX { get; set; }
        public IList<double> InY { get; set; }

        public IList<double> OutX { get; set; }
        public IList<double> OutY { get; set; }

        public IList<double> DYDX { get; set; }
        public IList<double> DY2DX2 { get; set; }
    }
}
