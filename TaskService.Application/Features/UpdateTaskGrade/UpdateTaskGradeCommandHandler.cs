using Grpc.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskService.Application.Interfaces;

namespace TaskService.Application.Features.UpdateTaskGrade
{
    public record UpdateTaskGradeCommand(Guid TaskId, Guid InternId, int NewScore) : IRequest<bool>;

    public class UpdateTaskGradeCommandHandler : IRequestHandler<UpdateTaskGradeCommand, bool>
    {
        private readonly ITaskRepository _repository;

        public UpdateTaskGradeCommandHandler(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateTaskGradeCommand request, CancellationToken cancellationToken)
        {
            var task = await _repository.GetByIdAsync(request.TaskId, cancellationToken);
            if (task == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Задача с ID {request.TaskId} не найдена."));
            }

            if (request.NewScore < 0 || request.NewScore > task.MaxScore)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, $"Оценка должна быть в диапазоне от 0 до {task.MaxScore}."));
            }

            var existingScore = await _repository.GetScoreAsync(request.TaskId, request.InternId, cancellationToken);
            if (existingScore == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Оценка для стажера {request.InternId} по задаче {request.TaskId} не найдена. Сначала выставите оценку через метод GradeTask."));
            }

            existingScore.Score = request.NewScore;

            await _repository.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
