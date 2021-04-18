using DataAnalysisSystem.DTO.AnalysisParametersDTO.Helpers;
using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.ParametersDetails
{
    public class DetailsDeriverativeParametersViewModel
    {
        public DetailsDeriverativeParametersViewModel()
        {

        }

        public IList<DatasetColumnSelectColumnForParametersTypeDoubleViewModel> NumberColumns { get; set; }
        public IList<DatasetColumnSelectColumnForParametersTypeStringViewModel> StringColumns { get; set; }
        public int ApproximationPointsNumber { get; set; }
    }
}
