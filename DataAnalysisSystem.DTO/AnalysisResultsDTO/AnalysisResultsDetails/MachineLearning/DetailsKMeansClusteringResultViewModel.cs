using DataAnalysisSystem.DTO.Helpers;
using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails
{
    public class DetailsKMeansClusteringResultViewModel
    {
        public DetailsKMeansClusteringResultViewModel()
        {
            this.NumberColumns = new List<DatasetContentKMeansClusteringResultsTypeDoubleViewModel>();
            this.StringColumns = new List<DatasetContentKMeansClusteringResultsTypeStringViewModel>();
            this.CentroidsPoints = new List<IList<double>>();
        }

        public IList<DatasetContentKMeansClusteringResultsTypeDoubleViewModel> NumberColumns { get; set; }
        public IList<DatasetContentKMeansClusteringResultsTypeStringViewModel> StringColumns { get; set; }
        public IList<ClusterMemberDataViewModel> Clusters { get; set; }
        public IList<IList<double>> CentroidsPoints { get; set; }

        public string OXAttributeName { get; set; }
        public string OYAttributeName { get; set; }

        public int OXCoordinatePosition { get; set; }
        public int OYCoordinatePosition { get; set; }
    }
}
