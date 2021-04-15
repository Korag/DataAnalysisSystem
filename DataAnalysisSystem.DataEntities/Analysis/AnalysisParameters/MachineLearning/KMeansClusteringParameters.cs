using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class KMeansClusteringParameters
    {
        public KMeansClusteringParameters()
        {

        }

        public IList<DatasetColumnSelectColumnForParametersTypeDouble> NumberColumns { get; set; }
        public IList<DatasetColumnSelectColumnForParametersTypeString> StringColumns { get; set; }
        public int ClustersNumber { get; set; }
    }
}