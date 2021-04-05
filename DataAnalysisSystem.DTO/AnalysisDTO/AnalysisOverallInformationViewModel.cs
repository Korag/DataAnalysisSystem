using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.AnalysisDTO
{
    public class AnalysisOverallInformationViewModel
    {
        public string AnalysisIdentificator { get; set; }

        [Display(Name = "Indexer")]
        public string AnalysisIndexer { get; set; }

        [Display(Name = "Analyses methods")]
        public IList<string> PerformedAnalysisMethods { get; set; }

        [Display(Name = "Creation at")]
        public string DateOfCreation { get; set; }

        [Display(Name = "Dataset name")]
        public string DatasetName { get; set; }

        [Display(Name = "File fullname")]
        public string OriginalDatasetFileFullName { get; set; }
    
        [Display(Name = "Dataset edited at")]
        public string DatasetDateOfEdition { get; set; }
    }
}
