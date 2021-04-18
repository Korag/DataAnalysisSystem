namespace DataAnalysisSystem.DTO.Helpers
{
    public class ClusterMemberDataViewModel
    {
        public int ClusterNumber { get; set; }
        public double OXValue { get; set; }
        public double OYValue { get; set; }
        public double[] DistancesToClusters { get; set; }
    }
}
