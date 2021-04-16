using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class ApproximationResult
    {
        public ApproximationResult()
        {
            this.NumberColumns = new List<DatasetContentApproximationResultsTypeDouble>();
            this.StringColumns = new List<DatasetContentApproximationResultsTypeString>();
        }

        public IList<DatasetContentApproximationResultsTypeDouble> NumberColumns { get; set; }
        public IList<DatasetContentApproximationResultsTypeString> StringColumns { get; set; }
        public int DataPointsNumber { get; set; }
        public int ApproximationPointsNumber { get; set; }
    }
}
