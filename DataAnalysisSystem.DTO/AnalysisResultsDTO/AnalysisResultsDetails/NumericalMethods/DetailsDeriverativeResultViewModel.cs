using DataAnalysisSystem.DTO.Helpers;
using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails
{
    public class DetailsDeriverativeResultViewModel
    {
        public DetailsDeriverativeResultViewModel()
        {
            this.NumberColumns = new List<DatasetContentDeriverativeResultsTypeDoubleViewModel>();
            this.StringColumns = new List<DatasetContentDeriverativeResultsTypeStringViewModel>();
        }

        public IList<DatasetContentDeriverativeResultsTypeDoubleViewModel> NumberColumns { get; set; }
        public IList<DatasetContentDeriverativeResultsTypeStringViewModel> StringColumns { get; set; }
        public int ApproximationPointsNumber { get; set; }
    }
}
