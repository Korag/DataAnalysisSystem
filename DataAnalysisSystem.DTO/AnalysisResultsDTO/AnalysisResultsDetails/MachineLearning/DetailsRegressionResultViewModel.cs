using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails
{
    public class DetailsRegressionResultViewModel
    {
        public DetailsRegressionResultViewModel(AnalysisResults analysisResults)
        {
            RegressionResult result = analysisResults.RegressionResult;

            if (result == null)
            {
                IsNull = true;
                return;
            }

            List<double> labels = new List<double>();
            List<ChartPoint> originalValuePoints = new List<ChartPoint>();
            List<ChartPoint> predictedValuePoints = new List<ChartPoint>();

            this.OXAttributeName = JsonConvert.SerializeObject(result.OXAttributeName);
            this.OYAttributeName = JsonConvert.SerializeObject(result.OYAttributeName);

            this.OXAttributeNameInfo = result.OXAttributeName;
            this.OYAttributeNameInfo = result.OYAttributeName;

            this.OXPredictionRegressionMetrics = new RegressionMetricViewModel()
            {
                LossFunction = Math.Round(result.OXPredictionRegressionMetrics.LossFunction, 4),
                MeanAbsoluteError = Math.Round(result.OXPredictionRegressionMetrics.MeanAbsoluteError, 4),
                MeanSquaredError = Math.Round(result.OXPredictionRegressionMetrics.MeanSquaredError, 4),
                RootMeanSquaredError = Math.Round(result.OXPredictionRegressionMetrics.RootMeanSquaredError, 4),
                RSquared = Math.Round(result.OXPredictionRegressionMetrics.RSquared, 4)
            };
            this.OYPredictionRegressionMetrics = new RegressionMetricViewModel()
            {
                LossFunction = Math.Round(result.OYPredictionRegressionMetrics.LossFunction, 4),
                MeanAbsoluteError = Math.Round(result.OYPredictionRegressionMetrics.MeanAbsoluteError, 4),
                MeanSquaredError = Math.Round(result.OYPredictionRegressionMetrics.MeanSquaredError, 4),
                RootMeanSquaredError = Math.Round(result.OYPredictionRegressionMetrics.RootMeanSquaredError, 4),
                RSquared = Math.Round(result.OYPredictionRegressionMetrics.RSquared, 4)
            };

            foreach (var singlePoint in result.PredictionResults)
            {
                ChartPoint cp = new ChartPoint
                {
                    x = Math.Round(singlePoint.OXValueFromSource, 4),
                    y = Math.Round(singlePoint.OYValueFromSource, 4)
                };

                ChartPoint cp2 = new ChartPoint
                {
                    x = Math.Round(singlePoint.OXValuePredicted, 4),
                    y = Math.Round(singlePoint.OYValuePredicted, 4)
                };

                originalValuePoints.Add(cp);
                predictedValuePoints.Add(cp2);

                labels.Add(cp.x);
                labels.Add(cp2.x);
            }

            labels = labels.OrderBy(z => z).ToList();
            labels = labels.Distinct().ToList();
            this.LabelsCount = labels.Count();

            Labels = JsonConvert.SerializeObject(labels);
            this.OriginalValuePoints = JsonConvert.SerializeObject(originalValuePoints);
            this.PredictedValuePoints = JsonConvert.SerializeObject(predictedValuePoints);
        }

        public bool IsNull { get; set; }

        public string OXAttributeName { get; set; }
        public string OYAttributeName { get; set; }

        [Display(Name = "OX attribute")]
        public string OXAttributeNameInfo { get; set; }

        [Display(Name = "OY attribute")]
        public string OYAttributeNameInfo { get; set; }

        public RegressionMetricViewModel OXPredictionRegressionMetrics { get; set; }
        public RegressionMetricViewModel OYPredictionRegressionMetrics { get; set; }

        public string OriginalValuePoints { get; set; }
        public string PredictedValuePoints { get; set; }

        public string Labels { get; set; }
        public int LabelsCount { get; set; }
    }
}