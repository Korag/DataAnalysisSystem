using Akka.Actor;
using Akka.Configuration;
using DataAnalysisSystem.AkkaNet.Actors;
using DataAnalysisSystem.AkkaNet.MessagesViewModels;
using DataAnalysisSystem.DataAnalysisMethods;
using DataAnalysisSystem.DataEntities;
using System;
using System.Collections.Generic;

namespace DataAnalysisSystem.AkkaNet
{
    public class ActorModelHub : IActorModelHub
    {
        private ActorSystem _localAkkaSystem { get; set; }
        private Dictionary<IAnalysisMethod, IActorRef> _commandMethodActorsByAnalysisMethod { get; set; }

        public ActorModelHub()
        {
            _commandMethodActorsByAnalysisMethod = new Dictionary<IAnalysisMethod, IActorRef>();
        }

        public void InitActorModelHub(ActorSystem akkaSystem)
        {
            this._localAkkaSystem = akkaSystem;
        }

        public void ExecuteAnalysisMethodCommandOnActor(DatasetContent datasetContent,
                                                        AnalysisParameters parameters,
                                                        IAnalysisMethod analysisMethod)
        {
            _commandMethodActorsByAnalysisMethod.TryGetValue(analysisMethod, out IActorRef commandActor);
            object message = new object();

            if (commandActor == null)
            {
                commandActor = _localAkkaSystem.ActorOf<CommandExecutorActor>("cea" + Guid.NewGuid());

                _commandMethodActorsByAnalysisMethod.Add(analysisMethod, commandActor);
            }

            message = new CommandExecutionRequestViewModel()
            {
                DatasetContent = datasetContent,
                Parameters = parameters,
                AnalysisMethod = analysisMethod
            };

            commandActor.Tell(message);
        }

        public AnalysisResults ReceiveObtainedSignalsFromActorModelSystem(IAnalysisMethod analysisMethod)
        {
            _commandMethodActorsByAnalysisMethod.TryGetValue(analysisMethod, out IActorRef commandActor);

            if (commandActor == null)
            {
                throw new EntryPointNotFoundException();
            }

            AnalysisResults results = null;

            do
            {
                results = commandActor.Ask<AnalysisResults>("GetAnalysisResults").GetAwaiter().GetResult();

            } while (results == null);

            commandActor.Tell("TerminateActor");

            _commandMethodActorsByAnalysisMethod.Remove(analysisMethod);

            return results;
        }
    }
}
