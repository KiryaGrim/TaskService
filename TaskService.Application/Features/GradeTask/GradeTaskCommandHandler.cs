using Grpc.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskService.Application.Interfaces;
using TaskService.Domain.Entities;

namespace TaskService.Application.Features.GradeTask
{
    public record GradeTaskCommand(Guid TaskId, Guid InternId, int Score) : IRequest<bool>;

    public class GradeTaskCommandHandler : IRequestHandler<GradeTaskCommand, bool>
    {
        private readonly ITaskRepository _repository;        

        public GradeTaskCommandHandler(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(GradeTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _repository.GetByIdAsync(request.TaskId, cancellationToken);
            if (task == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Задача с ID {request.TaskId} не найдена."));
            }

            if (request.Score < 0 || request.Score > task.MaxScore)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, $"Оценка должна быть в диапазоне от 0 до {task.MaxScore}."));
            }

            var existingScore = await _repository.GetScoreAsync(request.TaskId, request.InternId, cancellationToken);
            if (existingScore != null)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, $"Стажер {request.InternId} уже имеет оценку за задачу {request.TaskId}. Используйте метод изменения оценки."));
            }

            var taskScore = new TaskScore
            {
                Id = Guid.NewGuid(),
                TaskId = request.TaskId,
                InternId = request.InternId,
                Score = request.Score
            };

            await _repository.AddScoreAsync(taskScore, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
