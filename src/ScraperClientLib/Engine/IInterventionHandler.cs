namespace ScraperClientLib.Engine
{
    public interface IInterventionHandler
    {
        bool DoIntervention(ResponseDocument offendingTask);
        InterventionResult Handle(ResponseDocument offendingTask);
    }
}