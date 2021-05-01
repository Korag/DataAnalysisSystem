using Akka.Actor;
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
        private Dictionary<string, IActorRef> _commandMethodActorsByAnalysisMethod { get; set; }

        public ActorModelHub()
        {
            _commandMethodActorsByAnalysisMethod = new Dictionary<string, IActorRef>();
        }

        public void InitActorModelHub(ActorSystem akkaSystem)
        {
            this._localAkkaSystem = akkaSystem;
        }

        public void ExecuteAnalysisMethodCommandOnActor(DatasetContent datasetContent,
                                                        AnalysisParameters parameters,
                                                        IAnalysisMethod analysisMethod)
        {
            _commandMethodActorsByAnalysisMethod.TryGetValue(analysisMethod.ToString(), out IActorRef commandActor);
            object message = new object();

            if (commandActor == null)
            {
                commandActor = _localAkkaSystem.ActorOf<CommandExecutorActor>("cea" + Guid.NewGuid());

                _commandMethodActorsByAnalysisMethod.Add(analysisMethod.ToString(), commandActor);
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
            _commandMethodActorsByAnalysisMethod.TryGetValue(analysisMethod.ToString(), out IActorRef commandActor);

            if (commandActor == null)
            {
                throw new EntryPointNotFoundException();
            }

            AnalysisResults results = null;
            CommandExecutionResponseViewModel response = null;

            do
            {
                response = commandActor.Ask<CommandExecutionResponseViewModel>("GetAnalysisResults").GetAwaiter().GetResult();

            } while (response == null);

            results = response.AnalysisResult;
            commandActor.Tell("TerminateActor");

            _commandMethodActorsByAnalysisMethod.Remove(analysisMethod.ToString());

            return results;
        }
    }
}
