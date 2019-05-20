using System;
using System.Collections.Generic;
using System.Text;

namespace RealTimeTasks.Data
{
    public class Job
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public Status Status { get; set; }
        public User User { get; set; }
        public int? UserId { get; set; }
    }
}
