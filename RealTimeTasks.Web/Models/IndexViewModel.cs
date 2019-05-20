using RealTimeTasks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealTimeTasks.Web.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Job> Jobs { get; set; }
        public User User { get; set; }
    }
}
