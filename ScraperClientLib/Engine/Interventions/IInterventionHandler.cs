namespace ScraperClientLib.Engine.Interventions
{
    public interface IInterventionHandler
    {
        bool DoIntervention(ResponseContainer offendingTask);
        InterventionResult Handle(ResponseContainer offendingTask);
    }
}