using System.Net.Http;

namespace ScraperClientLib.Engine.Interventions
{
    public class InterventionResult
    {
        public InterventionResultState State { get; set; }
        public HttpRequestMessage IntermediateTask { get; set; }

        public InterventionResult(InterventionResultState state, HttpRequestMessage intermediateTask = null)
        {
            State = state;
            IntermediateTask = intermediateTask;
        }
    }
}
