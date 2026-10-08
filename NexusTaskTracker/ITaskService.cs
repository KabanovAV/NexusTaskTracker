namespace NexusTaskTracker
{
    public interface ITaskService
    {
        TaskTracker? GetTask(Guid id);
        List<TaskTracker> GetAllTask();
        List<TaskTracker> GetTaskByStatus(TaskStatus status);
        void MarkTaskStatus(Guid id, TaskStatus status);
        void AddTask(TaskTracker task);
        void UpdateTask(TaskTracker task);
        void RemoveTask(TaskTracker task);        
    }
}
