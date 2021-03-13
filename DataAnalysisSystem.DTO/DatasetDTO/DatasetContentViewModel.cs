using DataAnalysisSystem.DataEntities;
using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.DatasetDTO
{
    public class DatasetContentViewModel
    {
        public DatasetContentViewModel()
        {
            this.NumberColumns = new List<DatasetColumnTypeDouble>();
            this.StringColumns = new List<DatasetColumnTypeString>();
        }

        public IList<DatasetColumnTypeDouble> NumberColumns { get; set; }
        public IList<DatasetColumnTypeString> StringColumns { get; set; }
    }
}
