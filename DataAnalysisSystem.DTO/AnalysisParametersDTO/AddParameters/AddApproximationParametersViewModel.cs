using DataAnalysisSystem.DTO.DatasetDTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters
{
    public class AddApproximationParametersViewModel
    {
        private const int DEFAULT_APPROXIMATION_POINTS_NUMBER = 40;

        public AddApproximationParametersViewModel()
        {
            this.NumberColumns = new List<DatasetColumnSelectColumnForParametersTypeDoubleViewModel>();
            this.StringColumns = new List<DatasetColumnSelectColumnForParametersTypeStringViewModel>();
        }

        public AddApproximationParametersViewModel(DatasetContentViewModel datasestContent)
        {
            this.NumberColumns = new List<DatasetColumnSelectColumnForParametersTypeDoubleViewModel>();
            this.StringColumns = new List<DatasetColumnSelectColumnForParametersTypeStringViewModel>();
            
            this.ApproximationPointsNumber = DEFAULT_APPROXIMATION_POINTS_NUMBER;

            for (int i = 0; i < datasestContent.NumberColumns.Count() + datasestContent.StringColumns.Count(); i++)
            {
                var numberColumn = datasestContent.NumberColumns.Where(z => z.PositionInDataset == i).FirstOrDefault();

                if (numberColumn != null)
                {
                    this.NumberColumns.Add(new DatasetColumnSelectColumnForParametersTypeDoubleViewModel(
                                                             numberColumn.AttributeName, numberColumn.PositionInDataset, false));
                }
                else
                {
                    var stringColumn = datasestContent.StringColumns.Where(z => z.PositionInDataset == i).FirstOrDefault();

                    this.StringColumns.Add(new DatasetColumnSelectColumnForParametersTypeStringViewModel(
                                                             stringColumn.AttributeName, stringColumn.PositionInDataset, false));
                }
            }
        }

        public IList<DatasetColumnSelectColumnForParametersTypeDoubleViewModel> NumberColumns { get; set; }
        public IList<DatasetColumnSelectColumnForParametersTypeStringViewModel> StringColumns { get; set; }
       
        [Display(Name = "Number of approximation points")]
        [Range(typeof(int), "0", "2147483647")]
        [Required]
        public int ApproximationPointsNumber { get; set; }
    }
}
