using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.Helpers
{
    public class RegressionMetricViewModel
    {
        public RegressionMetricViewModel()
        {

        }

        [Display(Name="Mean absolute error")]
        public double MeanAbsoluteError { get; set; }

        [Display(Name = "Mean squared error")]
        public double MeanSquaredError { get; set; }

        [Display(Name = "Root mean squared error")]
        public double RootMeanSquaredError { get; set; }

        [Display(Name = "Loss function")]
        public double LossFunction { get; set; }

        [Display(Name = "RSquared")]
        public double RSquared { get; set; }
    }
}