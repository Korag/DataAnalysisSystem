using DataAnalysisSystem.DTO.AnalysisParametersDTO.Helpers;
using DataAnalysisSystem.DTO.DatasetDTO;
using System.Collections.Generic;
using System.Linq;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters
{
    public class AddHistogramParametersViewModel
    {
        private const int DEFAULT_RANGE = 10;

        public AddHistogramParametersViewModel()
        {
            this.NumberColumns = new List<DatasetContentSelectColumnForHistogramParametersTypeDoubleViewModel>();
            this.StringColumns = new List<DatasetContentSelectColumnForHistogramParametersTypeStringViewModel>();
        }

        public AddHistogramParametersViewModel(DatasetContentViewModel datasestContent)
        {
            this.NumberColumns = new List<DatasetContentSelectColumnForHistogramParametersTypeDoubleViewModel>();
            this.StringColumns = new List<DatasetContentSelectColumnForHistogramParametersTypeStringViewModel>();

            for (int i = 0; i < datasestContent.NumberColumns.Count() + datasestContent.StringColumns.Count(); i++)
            {
                var numberColumn = datasestContent.NumberColumns.Where(z => z.PositionInDataset == i).FirstOrDefault();

                if (numberColumn != null)
                {
                    DatasetContentSelectColumnForHistogramParametersTypeDoubleViewModel singleColumn = new DatasetContentSelectColumnForHistogramParametersTypeDoubleViewModel(
                                                                                                         numberColumn.AttributeName, numberColumn.PositionInDataset, false);
                    singleColumn.Range = DEFAULT_RANGE;

                    this.NumberColumns.Add(singleColumn);
                }
                else
                {
                    var stringColumn = datasestContent.StringColumns.Where(z => z.PositionInDataset == i).FirstOrDefault();
                    DatasetContentSelectColumnForHistogramParametersTypeStringViewModel singleColumn = new DatasetContentSelectColumnForHistogramParametersTypeStringViewModel(
                                                                                                         stringColumn.AttributeName, stringColumn.PositionInDataset, false);
                    this.StringColumns.Add(singleColumn);
                }
            }
        }

        public IList<DatasetContentSelectColumnForHistogramParametersTypeDoubleViewModel> NumberColumns { get; set; }
        public IList<DatasetContentSelectColumnForHistogramParametersTypeStringViewModel> StringColumns { get; set; }
    }
}

