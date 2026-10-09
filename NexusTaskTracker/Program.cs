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
if(task == null)
    Console.WriteLine("Задача не найдена!");
else
{
    task.Description = "Обновленная задача";
    service.UpdateTask(task);
    Console.WriteLine($"Обновлена задача: Описание - {task.Description} Статус - {task.Status}");
}

// Изменение статуса
service.MarkTaskStatus(id, TaskTrackerStatus.Done);

// Удаление задачи
service.RemoveTask(id);

// Получение задач
var tasks = service.GetAllTask();
foreach (var item in tasks)
    Console.WriteLine($"Id: {item.Id} Описание: {item.Description} Статус: {item.Status}");

Console.WriteLine();

var tasksStatus = service.GetTaskByStatus(TaskTrackerStatus.ToDo);
foreach (var item in tasksStatus)
    Console.WriteLine($"Id: {item.Id} Описание: {item.Description} Статус: {item.Status}");