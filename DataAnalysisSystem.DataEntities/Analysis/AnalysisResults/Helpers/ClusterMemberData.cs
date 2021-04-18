namespace DataAnalysisSystem.DataEntities
{
    public class ClusterMemberData
    {
        public int ClusterNumber { get; set; }
        public double OXValue { get; set; }
        public double OYValue { get; set; }
        public double[] DistancesToClusters { get; set; }
    }
}
