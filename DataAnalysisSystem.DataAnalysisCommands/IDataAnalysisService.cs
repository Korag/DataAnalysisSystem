using Akka.Actor;
using DataAnalysisSystem.DataAnalysisMethods;
using DataAnalysisSystem.DataEntities;

namespace DataAnalysisSystem.DataAnalysisCommands
{
    public interface IDataAnalysisService
    {
        public void InitService(DatasetContent datasetContent, AnalysisParameters analysisParameters, ActorSystem akkaSystem);
        public void SetAnalysisType(IAnalysisMethod method);
        public void RunAnalysis();
        public AnalysisResults GetResults();
    }
}