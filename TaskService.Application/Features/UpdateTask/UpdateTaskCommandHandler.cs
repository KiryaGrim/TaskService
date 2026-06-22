using Grpc.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskService.Application.Interfaces;

namespace TaskService.Application.Features.UpdateTask
{
    public record UpdateTaskCommand(Guid Id, string Title, int MaxScore) : IRequest<Unit>;

    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Unit>
    {
        private readonly ITaskRepository _repository;

        public UpdateTaskCommandHandler(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (task == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Задача с ID {request.Id} не найдена."));

            task.Title = request.Title;
            task.MaxScore = request.MaxScore;

            await _repository.UpdateAsync(task, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
