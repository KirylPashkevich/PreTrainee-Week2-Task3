using Task3.Models;

namespace Task3
{
    internal class Program
    {
       
        private static readonly ITodoTaskRepository _repository = new TodoTaskRepository();

        public static async Task Main(string[] args)
        {
            bool isRunning = true;
            Console.WriteLine("=== Консольное приложение 'Управление задачами' ===");

            while (isRunning)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Добавить новую задачу");
                Console.WriteLine("2. Просмотреть все задачи");
                Console.WriteLine("3. Обновить статус задачи (завершить/отменить)");
                Console.WriteLine("4. Удалить задачу по Id");
                Console.WriteLine("5. Выход");
                Console.Write("Ваш выбор: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    try
                    {
                        switch (choice)
                        {
                            case 1:
                                await AddNewTaskAsync();
                                break;
                            case 2:
                                await ViewAllTasksAsync();
                                break;
                            case 3:
                                await UpdateTaskStatusAsync();
                                break;
                            case 4:
                                await DeleteTaskAsync();
                                break;
                            case 5:
                                isRunning = false;
                                Console.WriteLine("Программа завершена.");
                                break;
                            default:
                                Console.WriteLine("Неверный ввод. Попробуйте снова.");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                      
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Ошибка операции: {ex.Message}");
                        Console.ResetColor();
                    }
                }
            }
        }

      
        private static async Task AddNewTaskAsync()
        {
            Console.Write("Введите заголовок задачи: ");
            string title = Console.ReadLine() ?? "Без заголовка";

            Console.Write("Введите описание задачи: ");
            string description = Console.ReadLine() ?? string.Empty;

            var newTask = new TodoTask
            {
                Title = title,
                Description = description,
                IsCompleted = false,
                CreatedAt = DateTime.Now 
            };

            int newId = await _repository.Add(newTask);
            Console.WriteLine($" Задача успешно добавлена с Id: {newId}");
        }

        private static async Task ViewAllTasksAsync()
        {
            var tasks = await _repository.GetAll();

            if (!tasks.Any())
            {
                Console.WriteLine("\nСписок задач пуст.");
                return;
            }

            Console.WriteLine("\n--- Список всех задач ---");
            foreach (var task in tasks)
            {
                string status = task.IsCompleted ? "[ГОТОВО]" : "[В РАБОТЕ]";
                Console.WriteLine($"[{task.Id}] {status} - {task.Title} (Создана: {task.CreatedAt:yyyy-MM-dd})");
                Console.WriteLine($"Описание: {task.Description}");
            }
            Console.WriteLine("-------------------------");
        }

        private static async Task UpdateTaskStatusAsync()
        {
            await ViewAllTasksAsync(); 
            Console.Write("Введите Id задачи для обновления статуса: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                

                Console.Write("Установить статус 'Завершено' (true/false)? ");
                if (bool.TryParse(Console.ReadLine(), out bool isCompleted))
                {
                  
                    var taskToUpdate = new TodoTask
                    {
                        Id = id,
                        
                        Title = "Placeholder",
                        Description = "Placeholder",
                        IsCompleted = isCompleted
                    };

                    await _repository.Update(taskToUpdate);
                    Console.WriteLine($"\n Статус задачи Id:{id} успешно обновлен на {isCompleted}");
                }
                else
                {
                    Console.WriteLine("Неверный ввод статуса (true/false).");
                }
            }
            else
            {
                Console.WriteLine("Неверный Id.");
            }
        }

        private static async Task DeleteTaskAsync()
        {
            await ViewAllTasksAsync(); 
            Console.Write("Введите Id задачи для удаления: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                await _repository.Delete(id);
                Console.WriteLine($"Задача Id:{id} успешно удалена.");
            }
            else
            {
                Console.WriteLine("Неверный Id.");
            }
        }
    }
}