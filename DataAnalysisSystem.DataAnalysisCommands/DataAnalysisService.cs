using DataAnalysisSystem.DataAnalysisMethods;
using DataAnalysisSystem.DataEntities;
using System;
using System.Collections.Generic;

namespace DataAnalysisSystem.DataAnalysisCommands
{
    public class DataAnalysisService : IDataAnalysisService
    {
        public IAnalysisMethod currentAnalysisMethod = null;
        protected DatasetContent _datasetContent = null;
        protected AnalysisParameters _analysisParameters = null;

        public void InitService(DatasetContent datasetContent,
                                AnalysisParameters analysisParameters)
        {
            this._datasetContent = datasetContent;
            this._analysisParameters = analysisParameters;
        }

        public void SetAnalysisType(IAnalysisMethod method)
        {
            this.currentAnalysisMethod = method;
        }

        public void RunAnalysis()
        {
            throw new NotImplementedException();
        }

        public List<AnalysisResults> GetResultsOfAllAnalyses()
        {
            throw new NotImplementedException();
        }
    }
}
