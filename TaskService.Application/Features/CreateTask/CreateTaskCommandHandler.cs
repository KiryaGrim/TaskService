using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskService.Application.Interfaces;
using DomainTask = TaskService.Domain.Entities.Task;

namespace TaskService.Application.Features.CreateTask
{
    public record CreateTaskCommand(Guid CourseId, string Title, int MaxScore) : IRequest<Guid>;

    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
    {
        private readonly ITaskRepository _repository;

        public CreateTaskCommandHandler(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = new DomainTask
            {
                Id = Guid.NewGuid(),
                CourseId = request.CourseId,
                Title = request.Title,
                MaxScore = request.MaxScore
            };

            await _repository.AddAsync(task, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return task.Id;
        }
    }
}
