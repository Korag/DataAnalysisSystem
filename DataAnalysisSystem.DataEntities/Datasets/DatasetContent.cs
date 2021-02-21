using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class DatasetContent
    {
        public DatasetContent()
        {
            this.NumberColumns = new List<DatasetColumnTypeDouble>();
            this.StringColumns = new List<DatasetColumnTypeString>();
        }

        public IList<DatasetColumnTypeDouble> NumberColumns { get; set; }
        public IList<DatasetColumnTypeString> StringColumns { get; set; }
    }
}
