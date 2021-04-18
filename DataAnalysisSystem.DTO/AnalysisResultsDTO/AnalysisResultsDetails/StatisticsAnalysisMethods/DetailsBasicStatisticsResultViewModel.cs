using DataAnalysisSystem.DTO.Helpers;
using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails
{
    public class DetailsBasicStatisticsResultViewModel
    {
        public DetailsBasicStatisticsResultViewModel()
        {
            this.NumberColumns = new List<DatasetContentBasicStatisticsResultsTypeDoubleViewModel>();
            this.StringColumns = new List<DatasetContentBasicStatisticsResultsTypeStringViewModel>();
        }

        public IList<DatasetContentBasicStatisticsResultsTypeDoubleViewModel> NumberColumns { get; set; }
        public IList<DatasetContentBasicStatisticsResultsTypeStringViewModel> StringColumns { get; set; }
        public int NumberColumnsAmount { get; set; }
        public int StringColumnsAmount { get; set; }
        public int NumberOfRows { get; set; }
    }
}
