using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails
{
    public class DetailsDeriverativeResultViewModel
    {
        public DetailsDeriverativeResultViewModel(AnalysisResults analysisResults)
        {
            DeriverativeResult result = analysisResults.DeriverativeResult;

            if (result == null)
            {
                IsNull = true;
                return;
            }

            this.AttributeName = new List<string>();
            this.FirstDeriverative = new List<string>();
            this.SecondDeriverative = new List<string>();

            this.OriginalValuePoints = new List<string>();
            this.ApproximatedValuePoints = new List<string>();

            this.Labels = new List<string>();

            foreach (var numberColumn in result.NumberColumns)
            {
                if (numberColumn.ColumnSelected)
                {
                    AttributeName.Add(JsonConvert.SerializeObject(numberColumn.AttributeName));

                    List<ChartPoint> firstDeriverativeValuePoints = new List<ChartPoint>();
                    List<ChartPoint> secondDeriverativeValuePoints = new List<ChartPoint>();

                    List<ChartPoint> originalValuePoints = new List<ChartPoint>();
                    List<ChartPoint> approximatedValuePoints = new List<ChartPoint>();
                    List<double> labels = new List<double>();

                    for (var i = 0; i < numberColumn.OutX.Count; i++)
                    {
                        ChartPoint cp = new ChartPoint
                        {
                            x = Math.Round(numberColumn.OutX[i], 4),
                            y = Math.Round(numberColumn.DYDX[i], 4)
                        };

                        firstDeriverativeValuePoints.Add(cp);

                        ChartPoint cp2 = new ChartPoint
                        {
                            x = Math.Round(numberColumn.OutX[i], 4),
                            y = Math.Round(numberColumn.DY2DX2[i], 4)
                        };

                        secondDeriverativeValuePoints.Add(cp2);

                        ChartPoint cp3 = new ChartPoint
                        {
                            x = Math.Round(numberColumn.OutX[i], 4),
                            y = Math.Round(numberColumn.OutY[i], 4)
                        };

                        approximatedValuePoints.Add(cp3);

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

                    labels = labels.OrderBy(z => z).ToList();
                    labels = labels.Distinct().ToList();

                    this.LabelsCount = labels.Count();

                    Labels.Add(JsonConvert.SerializeObject(labels));

                    FirstDeriverative.Add(JsonConvert.SerializeObject(firstDeriverativeValuePoints));
                    SecondDeriverative.Add(JsonConvert.SerializeObject(secondDeriverativeValuePoints));
                    OriginalValuePoints.Add(JsonConvert.SerializeObject(originalValuePoints));
                    ApproximatedValuePoints.Add(JsonConvert.SerializeObject(approximatedValuePoints));
                }
            }
        }

        public bool IsNull { get; set; }
        public IList<string> AttributeName { get; set; }

        public IList<string> OriginalValuePoints { get; set; }
        public IList<string> ApproximatedValuePoints { get; set; }

        public IList<string> FirstDeriverative { get; set; }
        public IList<string> SecondDeriverative { get; set; }

        public IList<string> Labels { get; set; }
        public int LabelsCount { get; set; }
    }
}