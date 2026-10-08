using System.Net.NetworkInformation;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace NexusTaskTracker
{
    public class TaskService : ITaskService
    {
        private readonly string _filePath;

        public TaskService(string filePath)
        {
            _filePath = filePath;
            if (!File.Exists(_filePath))
                using (File.Create(_filePath)) { }
        }

        public TaskTracker? GetTask(Guid id)
        {
            var json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                Console.WriteLine("Пустой список задач!");
                return null;
            }                
            else
            {
                var root = JsonSerializer.Deserialize<Root>(json) ?? new Root();
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
            var json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
                return [];
            else
            {
                var root = JsonSerializer.Deserialize<Root>(json) ?? new Root();
                return root.TaskTrackers;
            }
        }

        public List<TaskTracker> GetTaskByStatus(TaskStatus status)
        {
            var json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
                return [];
            else
            {
                var root = JsonSerializer.Deserialize<Root>(json) ?? new Root();
                return [.. root.TaskTrackers.Where(t => t.Status == status)];
            }
        }

        public void MarkTaskStatus(Guid id, TaskStatus status)
        {
            var json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
                Console.WriteLine("Пустой список задач!");
            else
            {
                var root = JsonSerializer.Deserialize<Root>(json) ?? new Root();
                var task = root.TaskTrackers.FirstOrDefault(t => t.Id == id);
                if (task == null)
                    Console.WriteLine("Задача не найдена!");
                else
                {
                    task.Status = status;
                    var options = new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic), WriteIndented = true };
                    File.WriteAllText(_filePath, JsonSerializer.Serialize(root, options));
                }
            }
        }

        public void AddTask(TaskTracker task)
        {
            Root root;
            var json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
                root = new();
            else
                root = JsonSerializer.Deserialize<Root>(json) ?? new Root();

            task.Status = TaskStatus.ToDo;
            task.CreatedAt = DateTime.Now;
            task.UpdatedAt = DateTime.Now;

            root.TaskTrackers.Add(task);
            var options = new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic), WriteIndented = true };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(root, options));
        }

        public void UpdateTask(TaskTracker task)
        {
            var json = File.ReadAllText(_filePath);
            var root = JsonSerializer.Deserialize<Root>(json);
            var uTask = root!.TaskTrackers.First(t => t.Id == task.Id);

            uTask.Description = task.Description;
            uTask.Status = task.Status;
            uTask.UpdatedAt = DateTime.Now;

            var options = new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic), WriteIndented = true };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(root, options));
        }

        public void RemoveTask(Guid id)
        {
            var json = File.ReadAllText(_filePath);
            var root = JsonSerializer.Deserialize<Root>(json);
            root!.TaskTrackers.RemoveAll(t => t.Id == id);

            var options = new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic), WriteIndented = true };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(root, options));
        }
    }
}
