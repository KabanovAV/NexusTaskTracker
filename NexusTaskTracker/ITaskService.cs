namespace NexusTaskTracker
{
    public interface ITaskService
    {
        List<TaskTracker> GetAllTask();
        List<TaskTracker> GetTaskByStatus(TaskStatus status);
        void MarkTaskStatus(TaskTracker task, TaskStatus status);
        void AddTask(TaskTracker task);
        void UpdateTask(TaskTracker task);
        void RemoveTask(TaskTracker task);        
    }
}
