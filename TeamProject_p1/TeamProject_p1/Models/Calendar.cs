using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TeamProject_p1.Models
{
    public class Calendar
    {
        public int CalendarId { get; set; }
        public DateTime TaskDate { get; set; }
    }
}
