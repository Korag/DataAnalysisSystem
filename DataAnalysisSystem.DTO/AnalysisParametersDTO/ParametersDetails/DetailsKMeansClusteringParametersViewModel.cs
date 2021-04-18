using DataAnalysisSystem.DTO.AnalysisParametersDTO.Helpers;
using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.ParametersDetails
{
    public class DetailsKMeansClusteringParametersViewModel
    {
        public DetailsKMeansClusteringParametersViewModel()
        {

        }

        public IList<DatasetColumnSelectColumnForParametersTypeDoubleViewModel> NumberColumns { get; set; }
        public IList<DatasetColumnSelectColumnForParametersTypeStringViewModel> StringColumns { get; set; }
        public int ClustersNumber { get; set; }
    }
}
