using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskService.Domain.Entities
{
    public class Task
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int MaxScore { get; set; }

        public List<TaskScore> TaskScores { get; set; } = new();
    }
}
