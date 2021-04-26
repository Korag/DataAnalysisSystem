using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class KMeansClusteringResult
    {
        public KMeansClusteringResult()
        {
            this.CentroidsPoints = new List<IList<double>>();
        }

        public IList<ClusterMemberData> Clusters { get; set; }
        public IList<IList<double>> CentroidsPoints { get; set; }

        public string OXAttributeName { get; set; }
        public string OYAttributeName { get; set; }

        public int OXCoordinatePosition { get; set; }
        public int OYCoordinatePosition { get; set; }
    }
}