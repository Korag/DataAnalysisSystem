using DataAnalysisSystem.DTO.AnalysisParametersDTO.Helpers;
using DataAnalysisSystem.DTO.DatasetDTO;
using System.Collections.Generic;
using System.Linq;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters
{
    public class AddBasicStatisticsParametersViewModel
    {
        public AddBasicStatisticsParametersViewModel()
        {
            this.NumberColumns = new List<DatasetColumnSelectColumnForParametersTypeDoubleViewModel>();
            this.StringColumns = new List<DatasetColumnSelectColumnForParametersTypeStringViewModel>();
        }

        public AddBasicStatisticsParametersViewModel(DatasetContentViewModel datasetContent)
        {
            this.NumberColumns = new List<DatasetColumnSelectColumnForParametersTypeDoubleViewModel>();
            this.StringColumns = new List<DatasetColumnSelectColumnForParametersTypeStringViewModel>();

            for (int i = 0; i < datasetContent.NumberColumns.Count() + datasetContent.StringColumns.Count(); i++)
            {
                var numberColumn = datasetContent.NumberColumns.Where(z => z.PositionInDataset == i).FirstOrDefault();

                if (numberColumn != null)
                {
                    this.NumberColumns.Add(new DatasetColumnSelectColumnForParametersTypeDoubleViewModel(
                                                             numberColumn.AttributeName, numberColumn.PositionInDataset, true));
                }
                else
                {
                    var stringColumn = datasetContent.StringColumns.Where(z => z.PositionInDataset == i).FirstOrDefault();

                    this.StringColumns.Add(new DatasetColumnSelectColumnForParametersTypeStringViewModel(
                                                             stringColumn.AttributeName, stringColumn.PositionInDataset, false));
                }
            }
        }

        public IList<DatasetColumnSelectColumnForParametersTypeDoubleViewModel> NumberColumns { get; set; }
        public IList<DatasetColumnSelectColumnForParametersTypeStringViewModel> StringColumns { get; set; }
    }
}
