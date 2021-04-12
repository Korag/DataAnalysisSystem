using DataAnalysisSystem.DTO.DatasetDTO;
using System.Collections.Generic;
using System.Linq;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters
{
    public class AddBasicStatisticsParametersViewModel
    {
        public AddBasicStatisticsParametersViewModel(DatasetContentViewModel datasestContent)
        {
            this.ColumnInfo = new List<BasicStatisticsColumnSelectedViewModel>();

            for (int i = 0; i < datasestContent.NumberColumns.Count() + datasestContent.StringColumns.Count(); i++)
            {
                BasicStatisticsColumnSelectedViewModel histColumn = new BasicStatisticsColumnSelectedViewModel();
                var numberColumn = datasestContent.NumberColumns.Where(z => z.PositionInDataset == i).FirstOrDefault();

                if (numberColumn != null)
                {
                    histColumn.AttributeName = numberColumn.AttributeName;

                }
                else
                {
                    var stringColumn = datasestContent.StringColumns.Where(z => z.PositionInDataset == i).FirstOrDefault();
                    histColumn.AttributeName = stringColumn.AttributeName;
                }

                this.ColumnInfo.Add(histColumn);
            }
        }

        public IList<BasicStatisticsColumnSelectedViewModel> ColumnInfo { get; set; }
    }
}
