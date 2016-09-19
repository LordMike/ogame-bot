using System.Net;
using ScraperClientLib.Engine;
using ScraperClientLib.Engine.Interventions;

namespace OgameBot.Engine.Interventions
{
    public class RetryIntervention : IInterventionHandler
    {
        private readonly int _retries;
        private ResponseContainer _lastTask;
        private int _currentRetryCount;

        public RetryIntervention(int retries = 3)
        {
            _retries = retries;
        }

        public bool DoIntervention(ResponseContainer offendingTask)
        {
            int statusCode = (int)offendingTask.StatusCode;

            if (500 <= statusCode && statusCode <= 599)
                // All kinds of path errors (gateway, server errors)
                return true;

            if (offendingTask.StatusCode == HttpStatusCode.RequestTimeout)
                // Rare timeouts
                return true;

            return false;
        }

        public InterventionResult Handle(ResponseContainer offendingTask)
        {
            if (_lastTask != offendingTask)
            {
                _lastTask = offendingTask;
                _currentRetryCount = 2;     // The initial request + this one

                return new InterventionResult(InterventionResultState.RetryCurrentTask);
            }

            _currentRetryCount++;
            if (_currentRetryCount >= _retries)
            {
                // Abort after _retries attempts
                return new InterventionResult(InterventionResultState.Abort);
            }

            return new InterventionResult(InterventionResultState.RetryCurrentTask);
        }
    }
}