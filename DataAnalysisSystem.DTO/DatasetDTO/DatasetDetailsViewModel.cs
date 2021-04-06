using DataAnalysisSystem.DTO.AnalysisDTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.DatasetDTO
{
    public class DatasetDetailsViewModel
    {
        public string DatasetIdentificator { get; set; }

        [Display(Name = "Dataset name")]
        public string DatasetName { get; set; }

        [Display(Name = "Creation at")]
        public string DateOfCreation { get; set; }
        [Display(Name = "Edition at")]
        public string DateOfEdition { get; set; }

        public DatasetContentViewModel DatasetContent { get; set; }
        public DatasetDetailsStatisticsViewModel DatasetStatistics { get; set; }

        public IList<DatasetDetailsAnalysisInformationViewModel> RelatedAnalyses { get; set; }
    }
}
