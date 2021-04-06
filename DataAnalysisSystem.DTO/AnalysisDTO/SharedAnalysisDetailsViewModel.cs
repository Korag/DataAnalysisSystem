using DataAnalysisSystem.DTO.AnalysisParametersDTO;
using DataAnalysisSystem.DTO.AnalysisResultsDTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.AnalysisDTO
{
    public class SharedAnalysisDetailsViewModel
    {
        public string AnalysisIdentificator { get; set; }
        public string DatasetIdentificator { get; set; }

        [Display(Name = "Indexer")]
        public string AnalysisIndexer { get; set; }

        [Display(Name = "Analyses methods")]
        public IList<string> PerformedAnalysisMethods { get; set; }

        [Display(Name = "Creation at")]
        public string DateOfCreation { get; set; }

        public bool UserHasAccessToDataset { get; set; }

        [Display(Name = "Analysis results")]
        public AnalysisResultsDetailsViewModel AnalysisResults { get; set; }

        [Display(Name = "Analysis parameters")]
        public AnalysisParametersDetailsViewModel AnalysisParameters { get; set; }
    }
}
