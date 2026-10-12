using System.Net.NetworkInformation;

namespace NexusTaskTracker
{
    public class TaskService : ITaskService
    {
        private readonly string _filePath;
        private readonly JsonFile _jsonFile;

        public TaskService(string filePath)
        {
            _filePath = filePath;
            _jsonFile = new();
        }

        public Result<TaskTracker> GetTask(Guid id)
        {
            var task = LoadRoot().TaskTrackers.FirstOrDefault(t => t.Id == id);
            return task ?? Result.Failure<TaskTracker>(Error.NotFound($"Задача с идентификатором '{id}' не найдена"));
        }

        public Result<List<TaskTracker>> GetAllTask()
            => LoadRoot().TaskTrackers;

        public Result<List<TaskTracker>> GetTaskByStatus(TaskTrackerStatus status)
            => LoadRoot().TaskTrackers.Where(t => t.Status == status).ToList();

        public Result MarkTaskStatus(Guid id, TaskTrackerStatus status)
        {
            var root = LoadRoot();
            var task = root.TaskTrackers.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.Status = status;
                _jsonFile.WriteJsonText(_filePath, root);
                return Result.Success();
            }
            return Result.Failure(Error.NotFound($"Задача с идентификатором '{id}' не найдена"));
        }

        public Result AddTask(TaskTracker task)
        {
            var root = LoadRoot();

            task.Status = TaskTrackerStatus.ToDo;
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;

            root.TaskTrackers.Add(task);
            _jsonFile.WriteJsonText(_filePath, root);

            return Result.Success();
        }

        public Result UpdateTask(TaskTracker task)
        {
            var root = LoadRoot();
            var uTask = root.TaskTrackers.FirstOrDefault(t => t.Id == task.Id);
            if (uTask != null)
            {
                uTask.Description = task.Description;
                uTask.UpdatedAt = DateTime.UtcNow;

                _jsonFile.WriteJsonText(_filePath, root);
                return Result.Success();
            }
            return Result.Failure(Error.NotFound($"Задача с идентификатором '{task.Id}' не найдена"));
        }

        public Result RemoveTask(Guid id)
        {
            var root = LoadRoot();
            var isRemoved = root.TaskTrackers.RemoveAll(t => t.Id == id);
            if (isRemoved > 0)
            {
                _jsonFile.WriteJsonText(_filePath, root);
                return Result.Success();
            }
            return Result.Failure(Error.NotFound($"Задача с идентификатором '{id}' не найдена"));
        }

        private Root LoadRoot()
        {
            var json = _jsonFile.ReadJsonText(_filePath);
            return string.IsNullOrWhiteSpace(json) ? new Root() : _jsonFile.DeserializeJson(json);
        }
    }
}
