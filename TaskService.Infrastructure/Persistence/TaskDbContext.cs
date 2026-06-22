using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TaskService.Domain.Entities;
using TaskService.Domain.Interfaces;
using DomainTask = TaskService.Domain.Entities.Task;

namespace TaskService.Infrastructure.Persistence
{
    public class TaskDbContext : DbContext, IUnitOfWork
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<DomainTask> Tasks => Set<DomainTask>();
        public DbSet<TaskScore> TaskScores => Set<TaskScore>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
