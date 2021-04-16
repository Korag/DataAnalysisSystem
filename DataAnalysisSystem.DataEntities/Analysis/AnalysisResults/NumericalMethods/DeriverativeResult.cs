using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{ 
    public class DeriverativeResult
    {
        public DeriverativeResult()
        {
            this.NumberColumns = new List<DatasetContentDeriverativeResultsTypeDouble>();
            this.StringColumns = new List<DatasetContentDeriverativeResultsTypeString>();
        }

        public IList<DatasetContentDeriverativeResultsTypeDouble> NumberColumns { get; set; }
        public IList<DatasetContentDeriverativeResultsTypeString> StringColumns { get; set; }
        public int ApproximationPointsNumber { get; set; }
    }
}