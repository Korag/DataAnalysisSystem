using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class RegressionResult
    {
        public RegressionResult()
        {
            this.PredictionResults = new List<RegressionPredictedValue>();
        }

        public string OXAttributeName { get; set; }
        public string OYAttributeName { get; set; }

        public int OXCoordinatePosition { get; set; }
        public int OYCoordinatePosition { get; set; }

        public RegressionMetric OXPredictionRegressionMetrics { get; set; }
        public RegressionMetric OYPredictionRegressionMetrics { get; set; }

        public IList<RegressionPredictedValue> PredictionResults { get; set; }
    }
}
