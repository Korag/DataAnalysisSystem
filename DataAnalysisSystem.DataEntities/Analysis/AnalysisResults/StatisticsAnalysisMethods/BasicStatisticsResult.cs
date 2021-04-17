using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class BasicStatisticsResult
    {
        public BasicStatisticsResult()
        {
            this.NumberColumns = new List<DatasetContentBasicStatisticsResultsTypeDouble>();
            this.StringColumns = new List<DatasetContentBasicStatisticsResultsTypeString>();
        }

        public IList<DatasetContentBasicStatisticsResultsTypeDouble> NumberColumns { get; set; }
        public IList<DatasetContentBasicStatisticsResultsTypeString> StringColumns { get; set; }
        public int NumberColumnsAmount { get; set; }
        public int StringColumnsAmount { get; set; }
        public int NumberOfRows { get; set; }     
    }
}
