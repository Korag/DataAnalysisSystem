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
            var config = ConfigurationFactory.ParseString(@"
                             akka.remote.dot-netty.tcp {
                             transport-class = ""Akka.Remote.Transport.DotNetty.DotNettyTransport, Akka.Remote""
                             transport-protocol = tcp
                             port = 8091
                             hostname = ""127.0.0.1""
                         }");

            _localAkkaSystem = ActorSystem.Create("local-akka-server", config);
            _commandMethodActorsByAnalysisMethod = new Dictionary<IAnalysisMethod, IActorRef>();
        }

        public void Dispose()
        {
            _localAkkaSystem.Dispose();
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
