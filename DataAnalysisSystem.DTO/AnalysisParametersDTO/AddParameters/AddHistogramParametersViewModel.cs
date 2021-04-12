using DataAnalysisSystem.DTO.DatasetDTO;
using System.Collections.Generic;
using System.Linq;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters
{
    public class AddHistogramParametersViewModel
    {
        private const int DATASET_CONTENT_ELEMENTS_NEEDED = 9;
        private const int DEFAULT_RANGE = 10;

        public AddHistogramParametersViewModel()
        {

        }

        public AddHistogramParametersViewModel(DatasetContentViewModel datasestContent)
        {
            this.DatasetContent = new DatasetContentViewModel();
            this.DatasetContent.NumberColumns = datasestContent.NumberColumns.Take(DATASET_CONTENT_ELEMENTS_NEEDED).ToList();
            this.DatasetContent.StringColumns = datasestContent.StringColumns.Take(DATASET_CONTENT_ELEMENTS_NEEDED).ToList();

            this.ColumnInfo = new List<HistogramColumnRangeViewModel>();

            for (int i = 0; i < this.DatasetContent.NumberColumns.Count() + this.DatasetContent.StringColumns.Count(); i++)
            {
                HistogramColumnRangeViewModel histColumn = new HistogramColumnRangeViewModel();
                var numberColumn = this.DatasetContent.NumberColumns.Where(z => z.PositionInDataset == i).FirstOrDefault();

                if (numberColumn != null)
                {
                    histColumn.AttributeName = numberColumn.AttributeName;

                }
                else
                {
                    var stringColumn = this.DatasetContent.StringColumns.Where(z => z.PositionInDataset == i).FirstOrDefault();
                    histColumn.AttributeName = stringColumn.AttributeName;
                }

                histColumn.Range = DEFAULT_RANGE;
                this.ColumnInfo.Add(histColumn);
            }
        }

        public DatasetContentViewModel DatasetContent { get; set; }
        public IList<HistogramColumnRangeViewModel> ColumnInfo { get; set; }
    }
}
