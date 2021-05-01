using Akka.Actor;
using DataAnalysisSystem.AkkaNet.MessagesViewModels;
using DataAnalysisSystem.DataEntities;
using System;

namespace DataAnalysisSystem.AkkaNet.Actors
{
    public class CommandExecutorActor : UntypedActor
    {
        private AnalysisResults _analysisResults = null;

        public CommandExecutorActor()
        {

        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case CommandExecutionRequestViewModel request:

                    try
                    {
                        _analysisResults = request.AnalysisMethod.GetDataAnalysisMethodResult(request.DatasetContent, request.Parameters);
                    }
                    catch (Exception e)
                    {
                        Context.Stop(Self);
                        break;
                    }
                    break;

                case "GetAnalysisResults":
                    Sender.Tell(new CommandExecutionResponseViewModel(_analysisResults));
                    break;

                case "TerminateActor":
                    Context.Stop(Self);
                    break;
            }
        }
    }
}
