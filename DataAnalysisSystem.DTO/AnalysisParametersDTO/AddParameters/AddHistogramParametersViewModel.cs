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
                                                                                                         numberColumn.AttributeName, numberColumn.PositionInDataset);
                    singleColumn.Range = DEFAULT_RANGE;
                    singleColumn.ColumnSelected = true;

                    this.NumberColumns.Add(singleColumn);
                }
                else
                {
                    var stringColumn = datasestContent.StringColumns.Where(z => z.PositionInDataset == i).FirstOrDefault();
                    DatasetContentSelectColumnForHistogramParametersTypeStringViewModel singleColumn = new DatasetContentSelectColumnForHistogramParametersTypeStringViewModel(
                                                                                                         stringColumn.AttributeName, stringColumn.PositionInDataset);
                    singleColumn.ColumnSelected = true;
                    this.StringColumns.Add(singleColumn);
                }
            }
        }

        public IList<DatasetContentSelectColumnForHistogramParametersTypeDoubleViewModel> NumberColumns { get; set; }
        public IList<DatasetContentSelectColumnForHistogramParametersTypeStringViewModel> StringColumns { get; set; }
    }
}

