using DataAnalysisSystem.DataEntities;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
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
                singleNumberColumnResult.Range = columnNumberParameters.Range;

                if (columnNumberParameters.ColumnSelected)
                {
                    double smallestValue = numberColumn.AttributeValue.OrderByDescending(z => z).Last();
                    double biggestValue = numberColumn.AttributeValue.OrderByDescending(z => z).First();

                    var histogram = new Histogram(numberColumn.AttributeValue, columnNumberParameters.Range, smallestValue, biggestValue);

                    for(int i = 0; i < singleNumberColumnResult.Range; i++)
                    {
                        singleNumberColumnResult.HistogramValues.Add(new HistogramNumberBin()
                        {
                            LowerBound = histogram[i].LowerBound,
                            UpperBound = histogram[i].UpperBound,
                            Width = histogram[i].Width,
                            Value = histogram[i].Count
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
                    Dictionary<string, double> categoricalDataDictionary = new Dictionary<string, double>();
                    List<double> codedCategoricalData = new List<double>();

                    int uniqueValuesCount = stringColumn.AttributeValue.Distinct().Count();

                    double j = 0;
                    foreach (var categoricalValue in stringColumn.AttributeValue.Distinct())
                    {
                        categoricalDataDictionary.Add(categoricalValue, j);
                        j += 1;
                    }

                    foreach (var categoricalValue in stringColumn.AttributeValue)
                    {
                        codedCategoricalData.Add(categoricalDataDictionary.Where(z=> z.Key == categoricalValue).Select(z=> z.Value).FirstOrDefault());
                    }

                    var histogram = new Histogram(codedCategoricalData, uniqueValuesCount, codedCategoricalData.Min(), codedCategoricalData.Max());

                    for (int i = 0; i < uniqueValuesCount; i++)
                    {
                        string categoricalValue = categoricalDataDictionary.ElementAt(i).Key;

                        singleStringColumnResult.HistogramValues.Add(new HistogramStringBin()
                        {
                            Bin = categoricalValue,
                            Value = Convert.ToInt32(histogram[i].Count)
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
