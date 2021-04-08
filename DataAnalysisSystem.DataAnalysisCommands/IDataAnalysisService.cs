using DataAnalysisSystem.DataAnalysisMethods;
using DataAnalysisSystem.DataEntities;

namespace DataAnalysisSystem.DataAnalysisCommands
{
    public interface IDataAnalysisService
    {
        public void InitService(DatasetContent datasetContent, AnalysisParameters analysisParameters);
        public void SetAnalysisType(IAnalysisMethod method);
        public void RunAnalysis();
        public AnalysisResults GetResults();
        public void Dispose();
    }
}