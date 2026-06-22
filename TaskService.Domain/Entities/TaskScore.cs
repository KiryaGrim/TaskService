using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskService.Domain.Entities
{
    public class TaskScore
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public Guid InternId { get; set; }
        public int? Score { get; set; }

        public Task Task { get; set; } = null!;
    }
}
