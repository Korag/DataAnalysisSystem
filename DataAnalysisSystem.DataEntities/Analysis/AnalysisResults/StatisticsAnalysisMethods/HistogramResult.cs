using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class HistogramResult
    {
        public HistogramResult()
        {
            this.NumberColumns = new List<DatasetContentHistogramResultsTypeDouble>();
            this.StringColumns = new List<DatasetContentHistogramResultsTypeString>();
        }

        public IList<DatasetContentHistogramResultsTypeDouble> NumberColumns { get; set; }
        public IList<DatasetContentHistogramResultsTypeString> StringColumns { get; set; }
    }
}
