using DataAnalysisSystem.DTO.AnalysisParametersDTO;

namespace DataAnalysisSystem.DTO.AnalysisDTO
{
    public class PerformNewAnalysisViewModel
    {
        public string DatasetIdentificator { get; set; }
        public string[] SelectedAnalysisMethods { get; set; }
        public AddAnalysisParametersViewModel AnalysisParameters { get; set; }
    }
}
