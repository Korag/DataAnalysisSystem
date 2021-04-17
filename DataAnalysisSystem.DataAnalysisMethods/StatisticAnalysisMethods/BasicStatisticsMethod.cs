using DataAnalysisSystem.DataEntities;
using MathNet.Numerics.Statistics;
using System.Linq;

namespace DataAnalysisSystem.DataAnalysisMethods
{
    public class BasicStatisticsMethod : IAnalysisMethod
    {
        public AnalysisResults GetDataAnalysisMethodResult(DatasetContent datasetContent, AnalysisParameters parameters)
        {
            AnalysisResults results = new AnalysisResults();
            results.BasicStatisticsResult = new BasicStatisticsResult();
            results.BasicStatisticsResult.NumberColumnsAmount = datasetContent.NumberColumns.Count();
            results.BasicStatisticsResult.StringColumnsAmount = datasetContent.StringColumns.Count();

            var firstColumn = datasetContent.NumberColumns.FirstOrDefault();

            if (firstColumn != null)
            {
                results.BasicStatisticsResult.NumberOfRows = firstColumn.AttributeValue.Count();
            }
            else
            {
                results.BasicStatisticsResult.NumberOfRows = datasetContent.StringColumns.FirstOrDefault().AttributeValue.Count();
            }

            foreach (var numberColumn in datasetContent.NumberColumns)
            {
                DatasetColumnSelectColumnForParametersTypeDouble columnNumberParameters = parameters.BasicStatisticsParameters.NumberColumns.Where(z => z.PositionInDataset == numberColumn.PositionInDataset).FirstOrDefault();
                DatasetContentBasicStatisticsResultsTypeDouble singleNumberColumnResult = new DatasetContentBasicStatisticsResultsTypeDouble(numberColumn.AttributeName, numberColumn.PositionInDataset, columnNumberParameters.ColumnSelected);

                if (columnNumberParameters.ColumnSelected)
                {
                    var statistics = new DescriptiveStatistics(numberColumn.AttributeValue);

                    singleNumberColumnResult.Max = statistics.Maximum;
                    singleNumberColumnResult.Min = statistics.Minimum;
                    singleNumberColumnResult.Mean = statistics.Mean;
                    singleNumberColumnResult.Variance = statistics.Variance;
                    singleNumberColumnResult.StdDev = statistics.StandardDeviation;
                    singleNumberColumnResult.Kurtosis = statistics.Kurtosis;
                    singleNumberColumnResult.Skewness = statistics.Skewness;
                    
                    singleNumberColumnResult.Median = Statistics.Median(numberColumn.AttributeValue);
                    singleNumberColumnResult.Covariance = Statistics.Covariance(numberColumn.AttributeValue, numberColumn.AttributeValue);
                    singleNumberColumnResult.LowerQuartile = Statistics.LowerQuartile(numberColumn.AttributeValue);
                    singleNumberColumnResult.UpperQuartile = Statistics.UpperQuartile(numberColumn.AttributeValue);
                }
                else
                {
                    singleNumberColumnResult.Max = 0;
                    singleNumberColumnResult.Min = 0;
                    singleNumberColumnResult.Mean = 0;
                    singleNumberColumnResult.Variance = 0;
                    singleNumberColumnResult.StdDev = 0;
                    singleNumberColumnResult.Kurtosis = 0;
                    singleNumberColumnResult.Skewness = 0;

                    singleNumberColumnResult.Median = 0;
                    singleNumberColumnResult.Covariance = 0;
                    singleNumberColumnResult.LowerQuartile = 0;
                    singleNumberColumnResult.UpperQuartile = 0;
                }

                results.BasicStatisticsResult.NumberColumns.Add(singleNumberColumnResult);
            }

            foreach (var stringColumn in datasetContent.StringColumns)
            {
                DatasetColumnSelectColumnForParametersTypeString columnStringParameters = parameters.BasicStatisticsParameters.StringColumns.Where(z => z.PositionInDataset == stringColumn.PositionInDataset).FirstOrDefault();
                DatasetContentBasicStatisticsResultsTypeString singleStringColumnResult = new DatasetContentBasicStatisticsResultsTypeString(stringColumn.AttributeName, stringColumn.PositionInDataset, columnStringParameters.ColumnSelected);
                results.BasicStatisticsResult.StringColumns.Add(singleStringColumnResult);
            }

            return results;
        }
    }
}
