using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class RegressionResult
    {
        public RegressionResult()
        {
            this.NumberColumns = new List<DatasetContentRegressionResultsTypeDouble>();
            this.StringColumns = new List<DatasetContentRegressionResultsTypeString>();
            this.PredictionResults = new List<RegressionPredictedValue>();
        }

        public IList<DatasetContentRegressionResultsTypeDouble> NumberColumns { get; set; }
        public IList<DatasetContentRegressionResultsTypeString> StringColumns { get; set; }

        public string OXAttributeName { get; set; }
        public string OYAttributeName { get; set; }

        public int OXCoordinatePosition { get; set; }
        public int OYCoordinatePosition { get; set; }

        public RegressionMetric OXPredictionRegressionMetrics { get; set; }
        public RegressionMetric OYPredictionRegressionMetrics { get; set; }

        public IList<RegressionPredictedValue> PredictionResults { get; set; }
    }
}
