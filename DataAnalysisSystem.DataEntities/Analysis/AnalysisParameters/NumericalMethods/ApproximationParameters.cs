using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class ApproximationParameters
    {
        public ApproximationParameters()
        {

        }

        public IList<DatasetColumnSelectColumnForParametersTypeDouble> NumberColumns { get; set; }
        public IList<DatasetColumnSelectColumnForParametersTypeString> StringColumns { get; set; }
        public int ApproximationPointsNumber { get; set; }
    }
}
