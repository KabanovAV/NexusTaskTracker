using NexusTaskTracker;

string filePath = "task-tracker.json";
TaskService service = new(filePath);

// Добавление задачи
Guid id = Guid.NewGuid();
TaskTracker nTask = new() { Id = id, Description = "Новая задача" };
service.AddTask(nTask);
Console.WriteLine($"Добавлена новая задача: Описание - {nTask.Description} Статус - {nTask.Status}");

// Обновление задачи
var task = service.GetTask(id);
if(task.IsFailure)
    Console.WriteLine(task.Error.Description);
else
{
    task.Value.Description = "Обновленная задача";
    service.UpdateTask(task.Value);
    Console.WriteLine($"Обновлена задача: Описание - {task.Value.Description} Статус - {task.Value.Status}");
}

// Изменение статуса
service.MarkTaskStatus(id, TaskTrackerStatus.Done);

// Удаление задачи
service.RemoveTask(id);

// Получение задач
var tasks = service.GetAllTask();
foreach (var item in tasks.Value)
    Console.WriteLine($"Id: {item.Id} Описание: {item.Description} Статус: {item.Status}");

Console.WriteLine();

var tasksStatus = service.GetTaskByStatus(TaskTrackerStatus.ToDo);
foreach (var item in tasksStatus.Value)
    Console.WriteLine($"Id: {item.Id} Описание: {item.Description} Статус: {item.Status}");