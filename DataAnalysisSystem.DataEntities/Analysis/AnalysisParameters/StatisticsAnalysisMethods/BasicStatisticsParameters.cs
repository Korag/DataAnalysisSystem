using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class BasicStatisticsParameters
    {
        public BasicStatisticsParameters()
        {

        }

        public IList<DatasetColumnSelectColumnForParametersTypeDouble> NumberColumns { get; set; }
        public IList<DatasetColumnSelectColumnForParametersTypeString> StringColumns { get; set; }
    }
}
