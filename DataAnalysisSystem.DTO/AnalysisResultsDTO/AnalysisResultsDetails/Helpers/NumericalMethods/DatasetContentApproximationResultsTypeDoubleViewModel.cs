using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.Helpers
{
    public class DatasetContentApproximationResultsTypeDoubleViewModel
    {
        public DatasetContentApproximationResultsTypeDoubleViewModel()
        {

        }

        public DatasetContentApproximationResultsTypeDoubleViewModel(string attributeName, int positionInDataset, bool columnSelected)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;
            this.ColumnSelected = columnSelected;

            this.InX = new List<double>();
            this.InY = new List<double>();

            this.OutX = new List<double>();
            this.OutY = new List<double>();
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }

        public bool ColumnSelected { get; set; }

        public IList<double> InX { get; set; }
        public IList<double> InY { get; set; }

        public IList<double> OutX { get; set; }
        public IList<double> OutY { get; set; }
    }
}
