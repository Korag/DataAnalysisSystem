using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.Helpers
{
    public class DatasetContentBasicStatisticsResultsTypeDoubleViewModel
    {
        public DatasetContentBasicStatisticsResultsTypeDoubleViewModel()
        {

        }

        public DatasetContentBasicStatisticsResultsTypeDoubleViewModel(string attributeName, int positionInDataset, bool columnSelected)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;
            this.ColumnSelected = columnSelected;
        }

        public bool ColumnSelected { get; set; }

        [Display(Name="Attribute name")]
        public string AttributeName { get; set; }

        [Display(Name="Position in dataset")]
        public int PositionInDataset { get; set; }

        [Display(Name = "Max")]
        public double Max { get; set; }

        [Display(Name = "Min")]
        public double Min { get; set; }

        [Display(Name = "Mean")]
        public double Mean { get; set; }

        [Display(Name = "Median")]
        public double Median { get; set; }

        [Display(Name = "Variance")]
        public double Variance { get; set; }

        [Display(Name = "Standard deviation")]
        public double StdDev { get; set; }

        [Display(Name = "Kurtosis")]
        public double Kurtosis { get; set; }

        [Display(Name = "Skewness")]
        public double Skewness { get; set; }

        [Display(Name = "Covariance")]
        public double Covariance { get; set; }

        [Display(Name = "Lower quartile")]
        public double LowerQuartile { get; set; }

        [Display(Name = "Upper quartile")]
        public double UpperQuartile { get; set; }
    }
}
