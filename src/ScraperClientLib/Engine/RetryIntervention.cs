using System.Net;

namespace ScraperClientLib.Engine
{
    public class RetryIntervention : IInterventionHandler
    {
        private readonly int _retries;
        private ResponseDocument _lastTask;
        private int _currentRetryCount;

        public RetryIntervention(int retries = 3)
        {
            _retries = retries;
        }

        public bool DoIntervention(ResponseDocument offendingTask)
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

        public InterventionResult Handle(ResponseDocument offendingTask)
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