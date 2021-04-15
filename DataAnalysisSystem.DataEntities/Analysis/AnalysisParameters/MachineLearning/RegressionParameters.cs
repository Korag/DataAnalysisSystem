using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class RegressionParameters
    {
        public RegressionParameters()
        {

        }

        public IList<DatasetColumnSelectColumnForParametersTypeDouble> NumberColumns { get; set; }
        public IList<DatasetColumnSelectColumnForParametersTypeString> StringColumns { get; set; }
    }
}
