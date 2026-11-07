using LessonService.Domain;
using Microsoft.EntityFrameworkCore;

namespace LessonService.Persistence
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
        {
            await dbContext.Database.MigrateAsync(cancellationToken);

            if (await dbContext.Categories.AnyAsync(cancellationToken))
                return;

            var oopCategory = new Category
            {
                Title = "Основы ООП",
                Description = "Основные принципы объектно-ориентированного программирования",
                OrderIndex = 1
            };

            var csharpCategory = new Category
            {
                Title = "Язык программирования C#",
                Description = "Синтаксис и основные возможности языка C#",
                OrderIndex = 2
            };

            var encapsulationLesson = new LessonModel
            {
                Title = "Инкапсуляция",
                Description = "Изучение принципа инкапсуляции в ООП",
                OrderIndex = 1,
                EstimatedMinutes = 20
            };

            encapsulationLesson.Sections.Add(new LessonSection
            {
                Title = "Что такое инкапсуляция?",
                ContentMarkdown =
                    "Инкапсуляция — это принцип ООП, который объединяет данные и методы для работы с ними и скрывает внутреннюю реализацию объекта.",
                CodeExample = """
                    public class Person
                    {
                        private string name;

                        public void SetName(string value)
                        {
                            name = value;
                        }
                    }
                    """,
                CodeLanguage = "csharp",
                OrderIndex = 1
            });

            encapsulationLesson.Sections.Add(new LessonSection
            {
                Title = "Зачем нужна инкапсуляция?",
                ContentMarkdown =
                    "Инкапсуляция позволяет контролировать доступ к внутреннему состоянию объекта и защищать его от некорректного изменения.",
                OrderIndex = 2
            });

            var inheritanceLesson = new LessonModel
            {
                Title = "Наследование",
                Description = "Изучение наследования и его применения",
                OrderIndex = 2,
                EstimatedMinutes = 25
            };

            var syntaxLesson = new LessonModel
            {
                Title = "Основы синтаксиса C#",
                Description = "Знакомство с базовым синтаксисом языка",
                OrderIndex = 1,
                EstimatedMinutes = 30
            };

            oopCategory.Lessons.Add(encapsulationLesson);
            oopCategory.Lessons.Add(inheritanceLesson);
            csharpCategory.Lessons.Add(syntaxLesson);

            dbContext.Categories.AddRange(oopCategory, csharpCategory);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
