using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskService.Domain.Entities;

namespace TaskService.Infrastructure.Persistence.Configurations
{
    public class TaskScoreConfiguration : IEntityTypeConfiguration<TaskScore>
    {
        public void Configure(EntityTypeBuilder<TaskScore> builder)
        {
            builder.ToTable("task_scores");

            builder.HasKey(ts => ts.Id);
            builder.Property(ts => ts.Id).HasColumnName("id");

            builder.Property(ts => ts.TaskId)
                .HasColumnName("task_id")
                .IsRequired();

            builder.Property(ts => ts.InternId)
                .HasColumnName("intern_id")
                .IsRequired();

            builder.Property(ts => ts.Score)
                .HasColumnName("score");

            builder.HasIndex(ts => ts.InternId)
                .HasDatabaseName("idx_task_scores_intern_id");

            builder.HasIndex(ts => new { ts.TaskId, ts.InternId })
                .IsUnique()
                .HasDatabaseName("uq_task_intern");
        }
    }
}
