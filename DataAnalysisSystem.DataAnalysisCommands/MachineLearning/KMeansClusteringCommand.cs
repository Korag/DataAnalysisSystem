using DataAnalysisSystem.DataAnalysisCommands.Abstract;
using DataAnalysisSystem.DataAnalysisMethods;
using DataAnalysisSystem.DataEntities;

namespace DataAnalysisSystem.DataAnalysisCommands
{
    public class KMeansClusteringCommand : AAnalysisCommand
    {
        public KMeansClusteringCommand(IDataAnalysisService analysisService) : base(analysisService)
        {

        }

        public override void RunAnalysis()
        {
            _analysisService.SetAnalysisType(new KMeansClusteringMethod());
            _analysisService.RunAnalysis();
        }

        public override AnalysisResults GetResults()
        {
            _analysisService.SetAnalysisType(new KMeansClusteringMethod());
            return _analysisService.GetResults();
        }
    }
}
