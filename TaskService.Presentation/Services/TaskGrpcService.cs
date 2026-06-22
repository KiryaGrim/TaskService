using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using TaskService.Application.Features.CreateTask;
using TaskService.Application.Features.DeleteTask;
using TaskService.Application.Features.GetCourseTasks;
using TaskService.Application.Features.GetCourseTasksWithScores;
using TaskService.Application.Features.GradeTask;
using TaskService.Application.Features.UpdateTask;
using TaskService.Application.Features.UpdateTaskGrade;
using TaskService.Presentation.Protos;

namespace TaskService.Presentation.Services;

public class TaskGrpcService : TaskService.Presentation.Protos.Task.TaskBase
{
    private readonly IMediator _mediator;

    public TaskGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Roles = "Admin")]
    public override async Task<CreateTaskResponse> CreateTask(CreateTaskRequest request, ServerCallContext context)
    {
        var command = new CreateTaskCommand(
            Guid.Parse(request.CourseId),
            request.Title,
            request.MaxScore
        );

        var taskId = await _mediator.Send(command, context.CancellationToken);

        return new CreateTaskResponse
        {
            TaskId = taskId.ToString(),
            Success = true
        };
    }

    [Authorize(Roles = "Admin")]
    public override async Task<DeleteTaskResponse> DeleteTask(DeleteTaskRequest request, ServerCallContext context)
    {
        var command = new DeleteTaskCommand(Guid.Parse(request.TaskId));

        await _mediator.Send(command, context.CancellationToken);

        return new DeleteTaskResponse
        {
            Success = true
        };
    }

    [Authorize(Roles = "Admin")]
    public override async Task<UpdateTaskResponse> UpdateTask(UpdateTaskRequest request, ServerCallContext context)
    {
        var command = new UpdateTaskCommand(
            Guid.Parse(request.TaskId),
            request.Title,
            request.MaxScore
        );

        await _mediator.Send(command, context.CancellationToken);

        return new UpdateTaskResponse
        {
            Success = true
        };
    }

    [Authorize(Roles = "Mentor")]
    public override async Task<GradeTaskResponse> GradeTask(GradeTaskRequest request, ServerCallContext context)
    {
        var command = new GradeTaskCommand(
            Guid.Parse(request.TaskId),
            Guid.Parse(request.InternId),
            request.Score
        );

        var success = await _mediator.Send(command, context.CancellationToken);

        return new GradeTaskResponse
        {
            Success = success
        };
    }

    [Authorize(Roles = "Mentor")]
    public override async Task<UpdateTaskGradeResponse> UpdateTaskGrade(UpdateTaskGradeRequest request, ServerCallContext context)
    {
        var command = new UpdateTaskGradeCommand(
            Guid.Parse(request.TaskId),
            Guid.Parse(request.InternId),
            request.NewScore
        );

        var success = await _mediator.Send(command, context.CancellationToken);

        return new UpdateTaskGradeResponse
        {
            Success = success
        };
    }

    [Authorize(Roles = "Admin")]
    public override async Task<GetCourseTasksResponse> GetCourseTasks(GetCourseTasksRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.CourseId, out var courseId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Некорректный формат CourseId. Ожидается GUID."));
        }

        var query = new GetCourseTasksCommand(courseId);
        var result = await _mediator.Send(query, context.CancellationToken);

        var response = new GetCourseTasksResponse
        {
            CourseId = request.CourseId,
            Success = true
        };

        response.Tasks.AddRange(result.Select(t => new CourseTaskItem
        {
            TaskId = t.Id.ToString(),
            Title = t.Title,
            MaxScore = t.MaxScore
        }));

        return response;
    }

    [Authorize]
    public override async Task<GetCourseTasksWithScoresResponse> GetCourseTasksWithScores(GetCourseTasksWithScoresRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.CourseId, out var courseId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Некорректный формат CourseId. Ожидается GUID."));
        }

        var query = new GetCourseTasksWithScoresQuery(courseId);
        var result = await _mediator.Send(query, context.CancellationToken);

        var response = new GetCourseTasksWithScoresResponse
        {
            CourseId = request.CourseId,
            Success = true
        };

        response.Tasks.AddRange(result.Select(t =>
        {
            var taskWithScores = new TaskWithScores
            {
                TaskId = t.TaskId.ToString(),
                Title = t.Title,
                MaxScore = t.MaxScore
            };

            taskWithScores.Scores.AddRange(t.Scores.Select(s => new InternScore
            {
                InternId = s.InternId.ToString(),
                Score = s.Score
            }));

            return taskWithScores;
        }));

        return response;
    }
}