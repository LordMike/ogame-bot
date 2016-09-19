namespace ScraperClientLib.Engine.Interventions
{
    public enum InterventionResultState
    {
        /// <summary>
        /// An exception is thrown
        /// </summary>
        Abort,

        /// <summary>
        /// The error is ignored
        /// </summary>
        Continue,

        /// <summary>
        /// The offending task will be retried
        /// </summary>
        RetryCurrentTask
    }
}