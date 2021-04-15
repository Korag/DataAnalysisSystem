using DataAnalysisSystem.DataEntities;
using Extreme.DataAnalysis;
using Extreme.Mathematics;
using System;
using System.Linq;

namespace DataAnalysisSystem.DataAnalysisMethods
{
    public class HistogramMethod : IAnalysisMethod
    {
        public AnalysisResults GetDataAnalysisMethodResult(DatasetContent datasetContent, AnalysisParameters parameters)
        {
            AnalysisResults results = new AnalysisResults();
            results.HistogramResult = new HistogramResult();

            foreach (var numberColumn in datasetContent.NumberColumns)
            {
                DatasetContentSelectColumnForHistogramParametersTypeDouble columnNumberParameters = parameters.HistogramParameters.NumberColumns.Where(z => z.PositionInDataset == numberColumn.PositionInDataset).FirstOrDefault();
                DatasetContentHistogramResultsTypeDouble singleNumberColumnResult = new DatasetContentHistogramResultsTypeDouble(numberColumn.AttributeName, numberColumn.PositionInDataset, columnNumberParameters.ColumnSelected);

                if (columnNumberParameters.ColumnSelected)
                {
                    int smallestValue = singleNumberColumnResult.AttributeName.OrderByDescending(z => z).Last();
                    int biggestValue = singleNumberColumnResult.AttributeName.OrderByDescending(z => z).First();

                    var histogram = Histogram.CreateEmpty<double>(smallestValue, biggestValue, columnNumberParameters.Range);
                    histogram.Tabulate<double>(numberColumn.AttributeValue);

                    foreach (var pair in histogram.BinsAndValues)
                    {
                        singleNumberColumnResult.HistogramValues.Add(new HistogramNumberBin()
                        {
                            Bin = Convert.ToInt32(pair.Key),
                            Value = Convert.ToInt32(pair.Value)
                        });
                    };
                }
                else
                {
                    singleNumberColumnResult.HistogramValues = null;
                }
                results.HistogramResult.NumberColumns.Add(singleNumberColumnResult);
            }

            foreach (var stringColumn in datasetContent.StringColumns)
            {
                DatasetContentSelectColumnForHistogramParametersTypeString columnStringParameters = parameters.HistogramParameters.StringColumns.Where(z => z.PositionInDataset == stringColumn.PositionInDataset).FirstOrDefault();
                DatasetContentHistogramResultsTypeString singleStringColumnResult = new DatasetContentHistogramResultsTypeString(stringColumn.AttributeName, stringColumn.PositionInDataset, columnStringParameters.ColumnSelected);

                if (columnStringParameters.ColumnSelected)
                {
                    var categoricalVector = Vector.CreateCategorical(stringColumn.AttributeValue);
                    var histogram = categoricalVector.CreateHistogram();

                    foreach (var pair in histogram.BinsAndValues)
                    {
                        singleStringColumnResult.HistogramValues.Add(new HistogramStringBin()
                        {
                            Bin = pair.Key,
                            Value = Convert.ToInt32(pair.Value)
                        });
                    };
                }
                else
                {
                    singleStringColumnResult.HistogramValues = null;
                }
                results.HistogramResult.StringColumns.Add(singleStringColumnResult);
            }

            return results;
        }
    }
}
