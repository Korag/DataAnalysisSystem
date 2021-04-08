using DataAnalysisSystem.DataAnalysisMethods;
using DataAnalysisSystem.DataEntities;

namespace DataAnalysisSystem.AkkaNet
{
    public interface IActorModelHub
    {
        public void ExecuteAnalysisMethodCommandOnActor(DatasetContent datasetContent, AnalysisParameters parameters, IAnalysisMethod analysisMethod);
        public AnalysisResults ReceiveObtainedSignalsFromActorModelSystem(IAnalysisMethod analysisMethod);
        public void Dispose();
    }
}
