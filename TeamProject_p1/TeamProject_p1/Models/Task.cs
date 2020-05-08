using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamProject_p1.Models
{
    public class DailyTask
    {
        public int DailyTaskId { get; set; }
        public int DateId { get; set; }
        public string Description { get; set; }
    }
}