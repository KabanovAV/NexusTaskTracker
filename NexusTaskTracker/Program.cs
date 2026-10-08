using NexusTaskTracker;

string filePath = "task-tracker.json";
TaskService service = new(filePath);

// Добавление задачи

//TaskTracker task = new() { Id = Guid.NewGuid(), Description = "Удалить задачу" };
//service.AddTask(task);

// Обновление задачи

//var id = Guid.Parse("cdab405e-76d9-4ee0-ae33-5aa410778ee8");
//var task = service.GetTask(id);
//task.Description = "Выключить плиту после готовки";
//service.UpdateTask(task);

// Удаление задачи

//var id = Guid.Parse("bc87c20d-c70a-495d-93ac-92bf3e903526");
//service.RemoveTask(id);

// Изменение статуса

//var id = Guid.Parse("feabd079-1aea-4081-88db-5d6361372a4f");
//service.MarkTaskStatus(id, TaskTrackerStatus.Done);

// Получение задач

var tasks = service.GetAllTask();
foreach (var task in tasks)
    Console.WriteLine($"Id: {task.Id} Description: {task.Description} Status: {task.Status}");

Console.WriteLine();

var tasksStatus = service.GetTaskByStatus(TaskTrackerStatus.ToDo);
foreach (var task in tasksStatus)
    Console.WriteLine($"Id: {task.Id} Description: {task.Description} Status: {task.Status}");