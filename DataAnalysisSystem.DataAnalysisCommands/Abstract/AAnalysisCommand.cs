using DataAnalysisSystem.DataEntities;
using System.Collections.Generic;

namespace DataAnalysisSystem.DataAnalysisCommands.Abstract
{
    public abstract class AAnalysisCommand
    {
        protected IDataAnalysisService _analysisService = null;
        protected DatasetContent _datasetContent = null;

        public AAnalysisCommand(IDataAnalysisService analysisService)
        {
            this._analysisService = analysisService;
        }

        public abstract void RunAnalysis();
        public abstract AnalysisResults GetResults();
    }
}
