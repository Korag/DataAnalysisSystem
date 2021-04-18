using DataAnalysisSystem.DTO.AnalysisParametersDTO.Helpers;
using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.ParametersDetails
{
    public class DetailsRegressionParametersViewModel
    {
        public DetailsRegressionParametersViewModel()
        {

        }

        public IList<DatasetColumnSelectColumnForParametersTypeDoubleViewModel> NumberColumns { get; set; }
        public IList<DatasetColumnSelectColumnForParametersTypeStringViewModel> StringColumns { get; set; }
    }
}
