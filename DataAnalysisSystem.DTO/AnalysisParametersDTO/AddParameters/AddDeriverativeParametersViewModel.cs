using DataAnalysisSystem.DTO.AnalysisParametersDTO.Helpers;
using DataAnalysisSystem.DTO.DatasetDTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters
{
    public class AddDeriverativeParametersViewModel
    {
        public AddDeriverativeParametersViewModel()
        {
            this.NumberColumns = new List<DatasetColumnSelectColumnForParametersTypeDoubleViewModel>();
            this.StringColumns = new List<DatasetColumnSelectColumnForParametersTypeStringViewModel>();
        }

        public AddDeriverativeParametersViewModel(DatasetContentViewModel datasetContent)
        {
            this.NumberColumns = new List<DatasetColumnSelectColumnForParametersTypeDoubleViewModel>();
            this.StringColumns = new List<DatasetColumnSelectColumnForParametersTypeStringViewModel>();

            for (int i = 0; i < datasetContent.NumberColumns.Count() + datasetContent.StringColumns.Count(); i++)
            {
                var numberColumn = datasetContent.NumberColumns.Where(z => z.PositionInDataset == i).FirstOrDefault();

                if (numberColumn != null)
                {
                    this.NumberColumns.Add(new DatasetColumnSelectColumnForParametersTypeDoubleViewModel(
                                                             numberColumn.AttributeName, numberColumn.PositionInDataset, false));
                }
                else
                {
                    var stringColumn = datasetContent.StringColumns.Where(z => z.PositionInDataset == i).FirstOrDefault();

                    this.StringColumns.Add(new DatasetColumnSelectColumnForParametersTypeStringViewModel(
                                                             stringColumn.AttributeName, stringColumn.PositionInDataset, false));
                }
            }

            var firstColumn = datasetContent.NumberColumns.FirstOrDefault();
            int datasetLength = 0;

            if (firstColumn != null)
            {
                datasetLength = firstColumn.AttributeValue.Count();
            }
            else
            {
                datasetLength = datasetContent.StringColumns.FirstOrDefault().AttributeValue.Count();
            }

            this.ApproximationPointsNumber = datasetLength;
        }

        public IList<DatasetColumnSelectColumnForParametersTypeDoubleViewModel> NumberColumns { get; set; }
        public IList<DatasetColumnSelectColumnForParametersTypeStringViewModel> StringColumns { get; set; }
        
        [Display(Name = "Number of approximation points")]
        [Range(typeof(int), "0", "2147483647")]
        [Required]
        public int ApproximationPointsNumber { get; set; }
    }
}
