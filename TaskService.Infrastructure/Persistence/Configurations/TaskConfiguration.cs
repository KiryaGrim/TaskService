using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = TaskService.Domain.Entities.Task;

namespace TaskService.Infrastructure.Persistence.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.ToTable("tasks");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id");

            builder.Property(t => t.CourseId)
                .HasColumnName("course_id")
                .IsRequired();

            builder.Property(t => t.Title)
                .HasColumnName("title")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(t => t.MaxScore)
                .HasColumnName("max_score")
                .IsRequired()
                .HasDefaultValue(0);

            builder.HasIndex(t => t.CourseId)
                .HasDatabaseName("idx_tasks_course_id");

            builder.HasMany(t => t.TaskScores)
                .WithOne(ts => ts.Task)
                .HasForeignKey(ts => ts.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
