using DataAnalysisSystem.DTO.Helpers;
using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails
{
    public class DetailsRegressionResultViewModel
    {
        public DetailsRegressionResultViewModel()
        {
            this.NumberColumns = new List<DatasetContentRegressionResultsTypeDoubleViewModel>();
            this.StringColumns = new List<DatasetContentRegressionResultsTypeStringViewModel>();
            this.PredictionResults = new List<RegressionPredictedValueViewModel>();
        }

        public IList<DatasetContentRegressionResultsTypeDoubleViewModel> NumberColumns { get; set; }
        public IList<DatasetContentRegressionResultsTypeStringViewModel> StringColumns { get; set; }

        public string OXAttributeName { get; set; }
        public string OYAttributeName { get; set; }

        public int OXCoordinatePosition { get; set; }
        public int OYCoordinatePosition { get; set; }

        public RegressionMetricViewModel OXPredictionRegressionMetrics { get; set; }
        public RegressionMetricViewModel OYPredictionRegressionMetrics { get; set; }

        public IList<RegressionPredictedValueViewModel> PredictionResults { get; set; }

        public bool IsNull = true;
    }
}
