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
            var root = LoadRoot();
            if (root != null)
            {
                var task = root.TaskTrackers.FirstOrDefault(t => t.Id == id);
                if (task != null)
                    return Result.Success(task);
                return Result.Failure<TaskTracker>(Error.NotFound($"Задача с идентификатором '{id}' не найдена"));
            }
            return Result.Failure<TaskTracker>(Error.Failure("В файле нет записей"));
        }

        public Result<List<TaskTracker>> GetAllTask()
        {
            var root = LoadRoot();
            if (root != null)
                return Result.Success(root.TaskTrackers);
            return Result.Success(new List<TaskTracker>());
        }

        public Result<List<TaskTracker>> GetTaskByStatus(TaskTrackerStatus status)
        {
            var root = LoadRoot();
            if (root != null)
                return Result.Success(root.TaskTrackers.Where(t => t.Status == status).ToList());
            return Result.Success(new List<TaskTracker>());
        }

        public Result MarkTaskStatus(Guid id, TaskTrackerStatus status)
        {
            var root = LoadRoot();
            if (root != null)
            {
                var task = root.TaskTrackers.FirstOrDefault(t => t.Id == id);
                if (task != null)
                {
                    task.Status = status;
                    _jsonFile.WriteJsonText(_filePath, root);
                    return Result.Success(task);
                }                
                return Result.Failure(Error.NotFound($"Задача с идентификатором '{id}' не найдена"));
            }
            return Result.Failure(Error.Failure("В файле нет записей"));
        }

        public Result AddTask(TaskTracker task)
        {
            var root = LoadRoot() ?? new();

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
            if (root != null)
            {
                var uTask = root.TaskTrackers.FirstOrDefault(t => t.Id == task.Id);
                if (uTask != null)
                {
                    uTask.Description = task.Description;
                    uTask.Status = task.Status;
                    uTask.UpdatedAt = DateTime.UtcNow;

                    _jsonFile.WriteJsonText(_filePath, root);
                    return Result.Success();
                }
                return Result.Failure(Error.NotFound($"Задача с идентификатором '{task.Id}' не найдена"));
            }
            return Result.Failure(Error.Failure("В файле нет записей"));
        }

        public Result RemoveTask(Guid id)
        {
            var root = LoadRoot();
            if (root != null)
            {
                var isRemoved = root.TaskTrackers.RemoveAll(t => t.Id == id);
                if (isRemoved > 0)
                {
                    _jsonFile.WriteJsonText(_filePath, root);
                    return Result.Success();
                }
                return Result.Failure(Error.NotFound($"Задача с идентификатором '{id}' не найдена"));
            }
            return Result.Failure(Error.Failure("В файле нет записей"));
        }

        private Root? LoadRoot()
        {
            var json = _jsonFile.ReadJsonText(_filePath);
            return string.IsNullOrWhiteSpace(json) ? null : _jsonFile.DeserializeJson(json);
        }
    }
}
