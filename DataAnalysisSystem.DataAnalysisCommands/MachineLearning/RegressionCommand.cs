using DataAnalysisSystem.DataAnalysisCommands.Abstract;
using DataAnalysisSystem.DataAnalysisMethods;
using DataAnalysisSystem.DataEntities;

namespace DataAnalysisSystem.DataAnalysisCommands
{
    public class RegressionCommand : AAnalysisCommand
    {
        public RegressionCommand(IDataAnalysisService analysisService) : base(analysisService)
        {

        }

        public override void RunAnalysis()
        {
            _analysisService.SetAnalysisType(new RegressionMethod());
            _analysisService.RunAnalysis();
        }

        public override AnalysisResults GetResults()
        {
            _analysisService.SetAnalysisType(new RegressionMethod());
            return _analysisService.GetResults();
        }
    }
}
