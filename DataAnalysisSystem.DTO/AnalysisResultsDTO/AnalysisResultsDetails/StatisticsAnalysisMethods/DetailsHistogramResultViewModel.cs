using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails
{
    public class DetailsHistogramResultViewModel
    {
        public DetailsHistogramResultViewModel(AnalysisResults analysisResults)
        {
            HistogramResult result = analysisResults.HistogramResult;

            if (result == null)
            {
                IsNull = true;
                return;
            }

            this.AttributeName = new List<string>();
            this.HistogramValues = new List<string>();
            this.Labels = new List<string>();
            this.LabelsCount = new List<int>();

            foreach (var numberColumn in result.NumberColumns)
            {
                if (numberColumn.ColumnSelected)
                {
                    AttributeName.Add(JsonConvert.SerializeObject(numberColumn.AttributeName));

                    List<ChartPointStringX> histogramPoints = new List<ChartPointStringX>();
                    List<string> labels = new List<string>();

                    for (var i = 0; i < numberColumn.HistogramValues.Count; i++)
                    {
                        ChartPointStringX cp = new ChartPointStringX
                        {
                            x = Math.Round(numberColumn.HistogramValues[i].LowerBound, 4).ToString() + " - " + Math.Round(numberColumn.HistogramValues[i].UpperBound, 4).ToString(),
                            y = numberColumn.HistogramValues[i].Value
                        };

                        histogramPoints.Add(cp);
                        labels.Add(cp.x);
                    }

                    labels = labels.OrderBy(z => z).ToList();
                    labels = labels.Distinct().ToList();

                    this.LabelsCount.Add(labels.Count());
                    Labels.Add(JsonConvert.SerializeObject(labels));

                    HistogramValues.Add(JsonConvert.SerializeObject(histogramPoints));
                }
            }

            foreach (var stringColumn in result.StringColumns)
            {
                if (stringColumn.ColumnSelected)
                {
                    AttributeName.Add(JsonConvert.SerializeObject(stringColumn.AttributeName));

                    List<ChartPointStringX> histogramPoints = new List<ChartPointStringX>();
                    List<string> labels = new List<string>();

                    for (var i = 0; i < stringColumn.HistogramValues.Count; i++)
                    {
                        ChartPointStringX cp = new ChartPointStringX
                        {
                            x = stringColumn.HistogramValues[i].Bin,
                            y = stringColumn.HistogramValues[i].Value
                        };

                        histogramPoints.Add(cp);
                        labels.Add(cp.x);
                    }

                    labels = labels.OrderBy(z => z).ToList();
                    labels = labels.Distinct().ToList();

                    this.LabelsCount.Add(labels.Count());
                    Labels.Add(JsonConvert.SerializeObject(labels));

                    HistogramValues.Add(JsonConvert.SerializeObject(histogramPoints));
                }
            }
        }

        public bool IsNull { get; set; }

        public IList<string> AttributeName { get; set; }
        public IList<string> HistogramValues { get; set; }

        public IList<string> Labels { get; set; }
        public IList<int> LabelsCount { get; set; }
    }
}