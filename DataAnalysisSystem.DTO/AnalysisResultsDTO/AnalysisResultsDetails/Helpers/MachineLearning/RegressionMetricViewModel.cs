﻿namespace DataAnalysisSystem.DTO.Helpers
{
    public class RegressionMetricViewModel
    {
        public RegressionMetricViewModel()
        {

        }

        public double MeanAbsoluteError { get; set; }
        public double MeanSquaredError { get; set; }
        public double RootMeanSquaredError { get; set; }
        public double LossFunction { get; set; }
        public double RSquared { get; set; }
    }
}