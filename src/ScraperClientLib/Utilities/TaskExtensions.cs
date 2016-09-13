using System;
using System.Threading.Tasks;

namespace ScraperClientLib.Utilities
{
    public static class TaskExtensions
    {
        public static void Sync(this Task task)
        {
            try
            {
                task.GetAwaiter().GetResult();
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public static T Sync<T>(this Task<T> task)
        {
            try
            {
                return task.GetAwaiter().GetResult();
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}