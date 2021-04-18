using DataAnalysisSystem.DTO.Helpers;
using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails
{
    public class DetailsApproximationResultViewModel
    {
        public DetailsApproximationResultViewModel()
        {
            this.NumberColumns = new List<DatasetContentApproximationResultsTypeDoubleViewModel>();
            this.StringColumns = new List<DatasetContentApproximationResultsTypeStringViewModel>();
        }

        public IList<DatasetContentApproximationResultsTypeDoubleViewModel> NumberColumns { get; set; }
        public IList<DatasetContentApproximationResultsTypeStringViewModel> StringColumns { get; set; }
    }
}