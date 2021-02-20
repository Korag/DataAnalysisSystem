using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities.Datasets
{
    public class DatasetContent
    {
        public DatasetContent()
        {
            this.NumberColumns = new List<DatasetColumnTypeDouble>();
            this.StringColumns = new List<DatasetColumnTypeString>();
        }

        public ICollection<DatasetColumnTypeDouble> NumberColumns { get; set; }
        public ICollection<DatasetColumnTypeString> StringColumns { get; set; }
    }
}
