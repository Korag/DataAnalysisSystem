using DataAnalysisSystem.DTO.AnalysisParametersDTO.Helpers;
using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.ParametersDetails
{
    public class DetailsHistogramParametersViewModel
    {
        public DetailsHistogramParametersViewModel()
        {

        }

        public IList<DatasetContentSelectColumnForHistogramParametersTypeDoubleViewModel> NumberColumns { get; set; }
        public IList<DatasetContentSelectColumnForHistogramParametersTypeStringViewModel> StringColumns { get; set; }
    }
}
