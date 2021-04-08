using DataAnalysisSystem.DataAnalysisCommands.Abstract;
using DataAnalysisSystem.DataAnalysisMethods;
using DataAnalysisSystem.DataEntities;

namespace DataAnalysisSystem.DataAnalysisCommands
{
    public class ApproximationCommand : AAnalysisCommand
    {
        public ApproximationCommand(IDataAnalysisService analysisService) : base(analysisService)
        {

        }

        public override void RunAnalysis()
        {
            _analysisService.SetAnalysisType(new ApproximationMethod());
            _analysisService.RunAnalysis();
        }

        public override AnalysisResults GetResults()
        {
            _analysisService.SetAnalysisType(new ApproximationMethod());
            return _analysisService.GetResults();
        }
    }
}
