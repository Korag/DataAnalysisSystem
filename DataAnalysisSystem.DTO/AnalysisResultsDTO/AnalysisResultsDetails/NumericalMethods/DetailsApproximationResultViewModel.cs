using DataAnalysisSystem.DataEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails
{
    public class DetailsApproximationResultViewModel
    {
        public DetailsApproximationResultViewModel(AnalysisResults analysisResults)
        {
            ApproximationResult result = analysisResults.ApproximationResult;

            this.AttributeName = new List<string>();
            this.ApproximatedValuePoints = new List<string>();
            this.OriginalValuePoints = new List<string>();
            this.Labels = new List<string>();

            foreach (var numberColumn in result.NumberColumns)
            {
                if (numberColumn.ColumnSelected)
                {
                    AttributeName.Add(JsonConvert.SerializeObject(numberColumn.AttributeName));

                    List<ChartPoint> approximatedValuePoints = new List<ChartPoint>();
                    List<ChartPoint> originalValuePoints = new List<ChartPoint>();
                    List<double> labels = new List<double>();

                    for (var i = 0; i < numberColumn.OutX.Count; i++)
                    {
                        ChartPoint cp = new ChartPoint
                        {
                            x = Math.Round(numberColumn.OutX[i], 4),
                            y = Math.Round(numberColumn.OutY[i], 4)
                        };

                        approximatedValuePoints.Add(cp);
                        labels.Add(cp.x);
                    }

                    for (var i = 0; i < numberColumn.InY.Count; i++)
                    {
                        ChartPoint cp = new ChartPoint
                        {
                            x = Math.Round(numberColumn.InX[i], 4),
                            y = Math.Round(numberColumn.InY[i], 4)
                        };

                        originalValuePoints.Add(cp);
                        labels.Add(cp.x);
                    }

                    labels = labels.OrderBy(z=> z).ToList();
                    labels = labels.Distinct().ToList();

                    this.LabelsCount = labels.Count();

                    Labels.Add(JsonConvert.SerializeObject(labels));

                    ApproximatedValuePoints.Add(JsonConvert.SerializeObject(approximatedValuePoints));
                    OriginalValuePoints.Add(JsonConvert.SerializeObject(originalValuePoints));
                }
            }
        }

        public IList<string> AttributeName { get; set; }

        public IList<string> ApproximatedValuePoints { get; set; }
        public IList<string> OriginalValuePoints { get; set; }
        public IList<string> Labels { get; set; }
        public int LabelsCount { get; set; }
    }
}