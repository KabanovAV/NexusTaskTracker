namespace NexusTaskTracker
{
    public interface ITaskService
    {
        TaskTracker? GetTask(Guid id);
        List<TaskTracker> GetAllTask();
        List<TaskTracker> GetTaskByStatus(TaskTrackerStatus status);
        void MarkTaskStatus(Guid id, TaskTrackerStatus status);
        void AddTask(TaskTracker task);
        void UpdateTask(TaskTracker task);
        void RemoveTask(Guid id);        
    }
}
