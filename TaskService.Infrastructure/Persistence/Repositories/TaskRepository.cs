using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskService.Application.Interfaces;
using TaskService.Domain.Entities;
using DomainTask = TaskService.Domain.Entities.Task;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Infrastructure.Persistence.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext _context;

        public TaskRepository(TaskDbContext context)
        {
            _context = context;
        }        

        public async Task AddAsync(DomainTask task, CancellationToken cancellationToken)
        {
            await _context.Tasks.AddAsync(task, cancellationToken);
        }

        public async Task UpdateAsync(DomainTask task, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(DomainTask task, CancellationToken cancellationToken)
        {
            _context.Tasks.Remove(task);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<DomainTask?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<TaskScore?> GetScoreAsync(Guid taskId, Guid internId, CancellationToken cancellationToken)
        {
            return await _context.TaskScores
                .FirstOrDefaultAsync(ts => ts.TaskId == taskId && ts.InternId == internId, cancellationToken);
        }

        public async Task AddScoreAsync(TaskScore score, CancellationToken cancellationToken)
        {
            await _context.TaskScores.AddAsync(score, cancellationToken);
        }

        public async Task<List<DomainTask>> GetByCourseIdWithScoresAsync(Guid courseId, CancellationToken cancellationToken)
        {
            return await _context.Tasks
                .Include(t => t.TaskScores)
                .Where(t => t.CourseId == courseId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<DomainTask>> GetByCourseIdAsync(Guid courseId, CancellationToken cancellationToken)
        {
            return await _context.Set<DomainTask>()
                .Where(t => t.CourseId == courseId)
                .ToListAsync(cancellationToken);
        }
    }
}
