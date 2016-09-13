namespace ScraperClientLib.Engine
{
    public interface IInterventionHandler
    {
        bool DoIntervention(ResponseContainer offendingTask);
        InterventionResult Handle(ResponseContainer offendingTask);
    }
}