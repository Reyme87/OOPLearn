using TaskService.Application.DTOs;
using TaskService.Application.Interfaces;
using TaskService.Application.Interfaces.Repositories;
using TaskService.Application.Interfaces.Services;
using TaskService.Domain;
using TaskService.Domain.Models;

namespace TaskService.Infrastructure.Services
{
    public class TaskSubmissionService : ITaskSubmissionService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskSubmissionRepository _submissionRepository;
        private readonly IAppDbContext _dbContext;

        public TaskSubmissionService(ITaskRepository taskRepository, ITaskSubmissionRepository submissionRepository, IAppDbContext dbContext)
        {
            _taskRepository = taskRepository;
            _submissionRepository = submissionRepository;
            _dbContext = dbContext;
        }

        public async Task<TaskSubmissionDto> SubmitAsync(SubmitTaskDto request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
                throw new ArgumentException("Не указан пользователь.");

            var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);

            if (task is null)
                throw new KeyNotFoundException("Задание не найдено.");

            // Выполнение пользовательского кода пока не поддерживается.
            if (task.Type == TaskType.Code)
            {
                throw new NotSupportedException("Проверка программного кода пока не реализована.");
            }

            var selectedIds = request.SelectedOptionIds
                .Distinct()
                .ToList();

            if (selectedIds.Count != request.SelectedOptionIds.Count)
                throw new ArgumentException("Варианты ответа не должны повторяться.");

            if (selectedIds.Count == 0)
                throw new ArgumentException("Необходимо выбрать ответ.");

            var selectedOptions = task.Options
                .Where(option => selectedIds.Contains(option.Id))
                .ToList();

            if (selectedOptions.Count != selectedIds.Count)
            {
                throw new ArgumentException("Один или несколько вариантов не принадлежат этому заданию.");
            }

            if (task.Type == TaskType.SingleChoice && selectedIds.Count != 1)
            {
                throw new ArgumentException("Для этого задания необходимо выбрать только один вариант.");
            }

            var correctIds = task.Options
                .Where(option => option.IsCorrect)
                .Select(option => option.Id)
                .ToHashSet();

            var submittedIds = selectedIds.ToHashSet();

            var isCorrect = correctIds.Count > 0 && correctIds.SetEquals(submittedIds);

            var submission = new TaskSubmission
            {
                TaskId = task.Id,
                UserId = request.UserId,
                Status = isCorrect ? SubmissionStatus.Accepted : SubmissionStatus.Reject,
                Score = isCorrect ? task.Points : 0,
                Feedback = isCorrect ? "Правильный ответ!" : "Ответ неверный.",
                SubmittedAt = DateTime.UtcNow,
                SelectedOptions = selectedIds.Select(optionId =>
                    new TaskSubmissionOption
                    {
                        TaskOptionId = optionId
                    }).ToList()
            };

            await _submissionRepository.AddAsync(submission, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return MapToDto(submission);
        }

        public async Task<TaskSubmissionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var submission = await _submissionRepository.GetByIdAsync(id, cancellationToken);

            return submission is null ? null : MapToDto(submission);
        }

        public async Task<List<TaskSubmissionDto>> GetByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var submissions = await _submissionRepository.GetByUserAsync(userId, cancellationToken);

            return submissions.Select(MapToDto).ToList();
        }

        private static TaskSubmissionDto MapToDto(TaskSubmission submission)
        {
            return new TaskSubmissionDto
            {
                Id = submission.Id,
                TaskId = submission.TaskId,
                Status = submission.Status,
                Score = submission.Score,
                Feedback = submission.Feedback,
                SubmittedAt = submission.SubmittedAt
            };
        }
    }
}
