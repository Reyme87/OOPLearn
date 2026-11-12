using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskService.Domain;
using TaskService.Domain.Models;

namespace TaskService.Persistence
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            AppDbContext context,
            IConfiguration configuration,
            CancellationToken cancellationToken = default)
        {
            await context.Database.MigrateAsync(cancellationToken);

            if (await context.Tasks.AnyAsync(cancellationToken))
                return;

            var lessonIdValue = configuration["SeedData:LessonId"];

            if (!Guid.TryParse(lessonIdValue, out var lessonId))
            {
                throw new InvalidOperationException(
                    "Не задан корректный SeedData:LessonId в appsettings.json.");
            }

            var singleChoiceTask = new TaskModel
            {
                LessonId = lessonId,
                Title = "Проверка знаний об инкапсуляции",
                Description = "Выберите правильное определение инкапсуляции.",
                Content = "Что означает инкапсуляция в ООП?",
                Type = TaskType.SingleChoice,
                Points = 5,
                OrderIndex = 1,
                Options = new List<TaskOption>
            {
                new()
                {
                    Text = "Объединение данных и методов работы с ними внутри класса.",
                    IsCorrect = true,
                    OrderIndex = 1
                },
                new()
                {
                    Text = "Создание нескольких классов с одинаковыми методами.",
                    IsCorrect = false,
                    OrderIndex = 2
                },
                new()
                {
                    Text = "Наследование свойств одного класса другим.",
                    IsCorrect = false,
                    OrderIndex = 3
                }
            }
            };

            var multipleChoiceTask = new TaskModel
            {
                LessonId = lessonId,
                Title = "Принципы ООП",
                Description = "Выберите все правильные варианты.",
                Content = "Какие из перечисленных понятий являются основными принципами ООП?",
                Type = TaskType.MultipleChoice,
                Points = 10,
                OrderIndex = 2,
                Options = new List<TaskOption>
            {
                new()
                {
                    Text = "Инкапсуляция",
                    IsCorrect = true,
                    OrderIndex = 1
                },
                new()
                {
                    Text = "Наследование",
                    IsCorrect = true,
                    OrderIndex = 2
                },
                new()
                {
                    Text = "Полиморфизм",
                    IsCorrect = true,
                    OrderIndex = 3
                },
                new()
                {
                    Text = "Компиляция",
                    IsCorrect = false,
                    OrderIndex = 4
                }
            }
            };

            var codeTask = new TaskModel
            {
                LessonId = lessonId,
                Title = "Первый метод",
                Description = "Напишите простой метод на C#.",
                Content = "Создайте метод, который возвращает сумму двух целых чисел.",
                Type = TaskType.Code,
                Points = 15,
                OrderIndex = 3,
                CodeLanguage = "csharp",
                StarterCode = "public int Sum(int a, int b)\n{\n    \n}",
                TestCases = new List<TaskTestCase>
            {
                new()
                {
                    Input = "2 3",
                    ExpectedOutput = "5",
                    IsHidden = false,
                    OrderIndex = 1
                },
                new()
                {
                    Input = "10 20",
                    ExpectedOutput = "30",
                    IsHidden = true,
                    OrderIndex = 2
                }
            }
            };

            context.Tasks.AddRange(
                singleChoiceTask,
                multipleChoiceTask,
                codeTask);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
