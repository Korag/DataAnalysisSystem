using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.AnalysisDTO
{
    public class DatasetDetailsAnalysisInformationViewModel
    {
        public string AnalysisIdentificator { get; set; }

        [Display(Name = "Name")]
        public string AnalysisName { get; set; }

        [Display(Name = "Indexer")]
        public string AnalysisIndexer { get; set; }

        [Display(Name = "Analyses methods")]
        public IList<string> PerformedAnalysisMethods { get; set; }

        [Display(Name = "Creation at")]
        public string DateOfCreation { get; set; }
    }
}
