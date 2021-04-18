using DataAnalysisSystem.DTO.AnalysisParametersDTO.Helpers;
using DataAnalysisSystem.DTO.DatasetDTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters
{
    public class AddKMeansClusteringParametersViewModel
    {
        public AddKMeansClusteringParametersViewModel()
        {
            this.NumberColumns = new List<DatasetColumnSelectColumnForParametersTypeDoubleViewModel>();
            this.StringColumns = new List<DatasetColumnSelectColumnForParametersTypeStringViewModel>();
        }

        public AddKMeansClusteringParametersViewModel(DatasetContentViewModel datasestContent)
        {
            this.NumberColumns = new List<DatasetColumnSelectColumnForParametersTypeDoubleViewModel>();
            this.StringColumns = new List<DatasetColumnSelectColumnForParametersTypeStringViewModel>();
            this.ClustersNumber = 3;

            for (int i = 0; i < datasestContent.NumberColumns.Count() + datasestContent.StringColumns.Count(); i++)
            {
                var numberColumn = datasestContent.NumberColumns.Where(z => z.PositionInDataset == i).FirstOrDefault();

                if (numberColumn != null)
                {
                    this.NumberColumns.Add(new DatasetColumnSelectColumnForParametersTypeDoubleViewModel(
                                                             numberColumn.AttributeName, numberColumn.PositionInDataset));
                }
                else
                {
                    var stringColumn = datasestContent.StringColumns.Where(z => z.PositionInDataset == i).FirstOrDefault();

                    this.StringColumns.Add(new DatasetColumnSelectColumnForParametersTypeStringViewModel(
                                                             stringColumn.AttributeName, stringColumn.PositionInDataset));
                }
            }
        }

        public IList<DatasetColumnSelectColumnForParametersTypeDoubleViewModel> NumberColumns { get; set; }
        public IList<DatasetColumnSelectColumnForParametersTypeStringViewModel> StringColumns { get; set; }
       
        [Display(Name="Number of clusters made")]
        [Range(typeof(int), "0", "2147483647")]
        [Required]
        public int ClustersNumber { get; set; }
    }
}
