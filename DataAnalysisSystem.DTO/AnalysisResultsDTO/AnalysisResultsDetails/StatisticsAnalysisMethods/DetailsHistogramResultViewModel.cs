using DataAnalysisSystem.DTO.Helpers;
using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails
{
    public class DetailsHistogramResultViewModel
    {
        public DetailsHistogramResultViewModel()
        {
            this.NumberColumns = new List<DatasetContentHistogramResultsTypeDoubleViewModel>();
            this.StringColumns = new List<DatasetContentHistogramResultsTypeStringViewModel>();
        }

        public IList<DatasetContentHistogramResultsTypeDoubleViewModel> NumberColumns { get; set; }
        public IList<DatasetContentHistogramResultsTypeStringViewModel> StringColumns { get; set; }

        public bool IsNull = true;
    }
}