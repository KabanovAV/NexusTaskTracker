namespace NexusTaskTracker
{
    public interface ITaskService
    {
        Result<TaskTracker> GetTask(Guid id);
        Result<List<TaskTracker>> GetAllTask();
        Result<List<TaskTracker>> GetTaskByStatus(TaskTrackerStatus status);
        Result MarkTaskStatus(Guid id, TaskTrackerStatus status);
        Result AddTask(TaskTracker task);
        Result UpdateTask(TaskTracker task);
        Result RemoveTask(Guid id);        
    }
}
