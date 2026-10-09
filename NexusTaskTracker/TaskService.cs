namespace NexusTaskTracker
{
    public class TaskService : ITaskService
    {
        private readonly string _filePath;
        private readonly JsonFile _jsonFile;

        public TaskService(string filePath)
        {
            _filePath = filePath;
            if (!File.Exists(_filePath))
                using (File.Create(_filePath)) { }
            _jsonFile = new();
        }

        public TaskTracker? GetTask(Guid id)
        {
            var json = _jsonFile.ReadJsonText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                Console.WriteLine("Пустой список задач!");
                return null;
            }
            else
            {
                var root = _jsonFile.DeserializeJson(json);
                var task = root.TaskTrackers.FirstOrDefault(t => t.Id == id);
                if (task == null)
                {
                    Console.WriteLine("Задача не найдена!");
                    return null;
                }
                return task;
            }
        }

        public List<TaskTracker> GetAllTask()
        {
            var json = _jsonFile.ReadJsonText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
                return [];
            else
            {
                var root = _jsonFile.DeserializeJson(json);
                return root.TaskTrackers;
            }
        }

        public List<TaskTracker> GetTaskByStatus(TaskTrackerStatus status)
        {
            var json = _jsonFile.ReadJsonText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
                return [];
            else
            {
                var root = _jsonFile.DeserializeJson(json);
                return [.. root.TaskTrackers.Where(t => t.Status == status)];
            }
        }

        public void MarkTaskStatus(Guid id, TaskTrackerStatus status)
        {
            var json = _jsonFile.ReadJsonText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
                Console.WriteLine("Пустой список задач!");
            else
            {
                var root = _jsonFile.DeserializeJson(json);
                var task = root.TaskTrackers.FirstOrDefault(t => t.Id == id);
                if (task == null)
                    Console.WriteLine("Задача не найдена!");
                else
                {
                    task.Status = status;
                    _jsonFile.WriteJsonText(_filePath, root);
                }
            }
        }

        public void AddTask(TaskTracker task)
        {
            Root root;
            var json = _jsonFile.ReadJsonText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
                root = new();
            else
                root = _jsonFile.DeserializeJson(json);

            task.Status = TaskTrackerStatus.ToDo;
            task.CreatedAt = DateTime.Now;
            task.UpdatedAt = DateTime.Now;

            root.TaskTrackers.Add(task);
            _jsonFile.WriteJsonText(json, root);
        }

        public void UpdateTask(TaskTracker task)
        {
            var json = _jsonFile.ReadJsonText(_filePath);
            var root = _jsonFile.DeserializeJson(json);
            var uTask = root!.TaskTrackers.First(t => t.Id == task.Id);

            uTask.Description = task.Description;
            uTask.Status = task.Status;
            uTask.UpdatedAt = DateTime.Now;

            _jsonFile.WriteJsonText(_filePath, root);
        }

        public void RemoveTask(Guid id)
        {
            var json = _jsonFile.ReadJsonText(_filePath);
            var root = _jsonFile.DeserializeJson(json);
            root!.TaskTrackers.RemoveAll(t => t.Id == id);
            _jsonFile.WriteJsonText(json, root);
        }
    }
}
