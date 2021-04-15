using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class HistogramParameters
    {
        public HistogramParameters()
        {

        }

        public IList<DatasetContentSelectColumnForHistogramParametersTypeDouble> NumberColumns { get; set; }
        public IList<DatasetContentSelectColumnForHistogramParametersTypeString> StringColumns { get; set; }
    }
}
