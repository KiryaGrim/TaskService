using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskService.Domain.Entities;
using DomainTask = TaskService.Domain.Entities.Task;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Application.Interfaces
{
    public interface ITaskRepository
    {
        Task AddAsync(DomainTask task, CancellationToken cancellationToken);
        Task UpdateAsync(DomainTask task, CancellationToken cancellationToken);
        Task DeleteAsync(DomainTask task, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task<DomainTask> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<TaskScore?> GetScoreAsync(Guid taskId, Guid internId, CancellationToken cancellationToken);
        Task AddScoreAsync(TaskScore score, CancellationToken cancellationToken);
        Task<List<DomainTask>> GetByCourseIdWithScoresAsync(Guid courseId, CancellationToken cancellationToken);
        Task<List<DomainTask>> GetByCourseIdAsync(Guid courseId, CancellationToken cancellationToken);
    }
}
