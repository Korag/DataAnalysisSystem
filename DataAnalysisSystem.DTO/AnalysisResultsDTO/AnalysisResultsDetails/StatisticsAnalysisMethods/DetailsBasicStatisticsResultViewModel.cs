using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.Helpers;
using System;
using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails
{
    public class DetailsBasicStatisticsResultViewModel
    {
        public DetailsBasicStatisticsResultViewModel(AnalysisResults analysisResults)
        {
            BasicStatisticsResult result = analysisResults.BasicStatisticsResult;

            if (result == null)
            {
                IsNull = true;
                return;
            }

            this.NumberColumns = new List<DatasetContentBasicStatisticsResultsTypeDoubleViewModel>();
            this.NumberColumnsAmount = result.NumberColumnsAmount;
            this.StringColumnsAmount = result.StringColumnsAmount;
            this.NumberOfRows = result.NumberOfRows;

            foreach (var numberColumn in result.NumberColumns)
            {
                DatasetContentBasicStatisticsResultsTypeDoubleViewModel singleColumn = new DatasetContentBasicStatisticsResultsTypeDoubleViewModel(numberColumn.AttributeName, numberColumn.PositionInDataset, numberColumn.ColumnSelected);

                if (numberColumn.ColumnSelected)
                {
                    singleColumn.Max = Math.Round(numberColumn.Max, 4);
                    singleColumn.Min = Math.Round(numberColumn.Min, 4);
                    singleColumn.Mean = Math.Round(numberColumn.Mean, 4);
                    singleColumn.Median = Math.Round(numberColumn.Median, 4);
                    singleColumn.Variance = Math.Round(numberColumn.Variance, 4);
                    singleColumn.StdDev = Math.Round(numberColumn.StdDev, 4);
                    singleColumn.Kurtosis = Math.Round(numberColumn.Kurtosis, 4);
                    singleColumn.Skewness = Math.Round(numberColumn.Skewness, 4);
                    singleColumn.Covariance = Math.Round(numberColumn.Covariance, 4);
                    singleColumn.LowerQuartile = Math.Round(numberColumn.LowerQuartile, 4);
                    singleColumn.UpperQuartile = Math.Round(numberColumn.UpperQuartile, 4);
                }

                NumberColumns.Add(singleColumn);
            }
        }

        public bool IsNull { get; set; }

        public IList<DatasetContentBasicStatisticsResultsTypeDoubleViewModel> NumberColumns { get; set; }
        public int NumberColumnsAmount { get; set; }
        public int StringColumnsAmount { get; set; }
        public int NumberOfRows { get; set; }
    }
}
