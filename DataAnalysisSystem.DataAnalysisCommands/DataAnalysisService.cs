using Akka.Actor;
using DataAnalysisSystem.AkkaNet;
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

        protected ActorModelHub _actorModelHub = null;

        public DataAnalysisService()
        {

        }

        public void InitService(DatasetContent datasetContent,
                                AnalysisParameters analysisParameters,
                                ActorSystem akkaSystem)
        {
            this._datasetContent = datasetContent;
            this._analysisParameters = analysisParameters;

            _actorModelHub = new ActorModelHub();
            _actorModelHub.InitActorModelHub(akkaSystem);
        }

        public void SetAnalysisType(IAnalysisMethod method)
        {
            this.currentAnalysisMethod = method;
        }

        public void RunAnalysis()
        {
            _actorModelHub.ExecuteAnalysisMethodCommandOnActor(_datasetContent, _analysisParameters, currentAnalysisMethod);
        }

        public AnalysisResults GetResults()
        {
            return _actorModelHub.ReceiveObtainedSignalsFromActorModelSystem(currentAnalysisMethod);
        }
    }
}
