using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskService.Application.Interfaces;
using DomainTask = TaskService.Domain.Entities.Task;

namespace TaskService.Application.Features.GetCourseTasksWithScores
{
    public record InternScoreDto(Guid InternId, int Score);

    public record TaskWithScoresDto(Guid TaskId, string Title, int MaxScore, List<InternScoreDto> Scores);

    public record GetCourseTasksWithScoresQuery(Guid CourseId) : IRequest<List<TaskWithScoresDto>>;

    public class GetCourseTasksWithScoresQueryHandler : IRequestHandler<GetCourseTasksWithScoresQuery, List<TaskWithScoresDto>>
    {
        private readonly ITaskRepository _repository;

        public GetCourseTasksWithScoresQueryHandler(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TaskWithScoresDto>> Handle(GetCourseTasksWithScoresQuery request, CancellationToken cancellationToken)
        {
            List<DomainTask> tasks = await _repository.GetByCourseIdWithScoresAsync(request.CourseId, cancellationToken);

            return tasks.Select(t => new TaskWithScoresDto(
                t.Id,
                t.Title,
                t.MaxScore,
                t.TaskScores
                    .Where(s => s.Score.HasValue)
                    .Select(s => new InternScoreDto(
                        s.InternId,
                        s.Score!.Value
                    ))
                    .ToList()
            )).ToList();
        }
    }
}
