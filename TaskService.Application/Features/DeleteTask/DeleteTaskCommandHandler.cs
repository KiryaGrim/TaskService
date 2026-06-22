using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskService.Application.Interfaces;

namespace TaskService.Application.Features.DeleteTask
{
    public record DeleteTaskCommand(Guid Id) : IRequest<Unit>;

    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Unit>
    {
        private readonly ITaskRepository _repository;

        public DeleteTaskCommandHandler(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (task == null)
                throw new Exception($"Task with ID {request.Id} not found.");

            await _repository.DeleteAsync(task, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
