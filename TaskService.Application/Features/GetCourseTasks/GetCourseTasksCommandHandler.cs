using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskService.Application.Interfaces;
using DomainTask = TaskService.Domain.Entities.Task;

namespace TaskService.Application.Features.GetCourseTasks
{
    public record CourseTaskDto(Guid Id, string Title, int MaxScore);

    public record GetCourseTasksCommand(Guid CourseId) : IRequest<List<CourseTaskDto>>;

    public class GetCourseTasksCommandHandler : IRequestHandler<GetCourseTasksCommand, List<CourseTaskDto>>
    {
        private readonly ITaskRepository _repository;

        public GetCourseTasksCommandHandler(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CourseTaskDto>> Handle(GetCourseTasksCommand request, CancellationToken cancellationToken)
        {
            List<DomainTask> tasks = await _repository.GetByCourseIdAsync(request.CourseId, cancellationToken);

            return tasks.Select(t => new CourseTaskDto(
                t.Id,
                t.Title,
                t.MaxScore
            )).ToList();
        }
    }
}
