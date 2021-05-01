using DataAnalysisSystem.DataEntities;

namespace DataAnalysisSystem.DataAnalysisCommands.Abstract
{
    public abstract class AAnalysisCommand
    {
        protected IDataAnalysisService _analysisService = null;

        public AAnalysisCommand(IDataAnalysisService analysisService)
        {
            this._analysisService = analysisService;
        }

        public abstract void RunAnalysis();
        public abstract AnalysisResults GetResults();
    }
}
