using DataAnalysisSystem.DTO.DatasetDTO;
using System.Collections.Generic;
using System.Linq;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters
{
    public class AddBasicStatisticsParametersViewModel
    {
        public AddBasicStatisticsParametersViewModel()
        {
            this.NumberColumns = new List<DatasetColumnSelectColumnForParametersTypeDouble>();
            this.StringColumns = new List<DatasetColumnSelectColumnForParametersTypeString>();
        }

        public AddBasicStatisticsParametersViewModel(DatasetContentViewModel datasestContent)
        {
            this.NumberColumns = new List<DatasetColumnSelectColumnForParametersTypeDouble>();
            this.StringColumns = new List<DatasetColumnSelectColumnForParametersTypeString>();

            for (int i = 0; i < datasestContent.NumberColumns.Count() + datasestContent.StringColumns.Count(); i++)
            {
                var numberColumn = datasestContent.NumberColumns.Where(z => z.PositionInDataset == i).FirstOrDefault();

                if (numberColumn != null)
                {
                    this.NumberColumns.Add(new DatasetColumnSelectColumnForParametersTypeDouble(
                                                             numberColumn.AttributeName, numberColumn.PositionInDataset, numberColumn.AttributeValue.Take(0).ToList()));
                }
                else
                {
                    var stringColumn = datasestContent.StringColumns.Where(z => z.PositionInDataset == i).FirstOrDefault();

                    this.StringColumns.Add(new DatasetColumnSelectColumnForParametersTypeString(
                                                             stringColumn.AttributeName, stringColumn.PositionInDataset, stringColumn.AttributeValue.Take(0).ToList()));
                }
            }
        }

        public IList<DatasetColumnSelectColumnForParametersTypeDouble> NumberColumns { get; set; }
        public IList<DatasetColumnSelectColumnForParametersTypeString> StringColumns { get; set; }
    }
}
