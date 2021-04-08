using DataAnalysisSystem.DataAnalysisCommands.Abstract;
using DataAnalysisSystem.DataAnalysisMethods;
using DataAnalysisSystem.DataEntities;

namespace DataAnalysisSystem.DataAnalysisCommands
{
    public class DeriveretiveCommand : AAnalysisCommand
    {
        public DeriveretiveCommand(IDataAnalysisService analysisService) : base(analysisService)
        {

        }

        public override void RunAnalysis()
        {
            _analysisService.SetAnalysisType(new DeriverativeMethod());
            _analysisService.RunAnalysis();
        }

        public override AnalysisResults GetResults()
        {
            _analysisService.SetAnalysisType(new DeriverativeMethod());
            return _analysisService.GetResults();
        }
    }
}
