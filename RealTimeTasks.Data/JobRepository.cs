using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RealTimeTasks.Data
{
    public class JobRepository
    {
        private string _connectionString;

        public JobRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int AddJob(Job job)
        {
            using (JobContext ctx = new JobContext(_connectionString))
            {
                ctx.Jobs.Add(job);
                ctx.SaveChanges();
                return job.Id;
            }
        }

        public IEnumerable<Job> GetJobs()
        {
            using (JobContext ctx = new JobContext(_connectionString))
            {
                return ctx.Jobs.Include(j => j.User).Where(j => j.Status != Status.Complete).ToList();
            }
        }

        public Job AssignJob(int jobId, int userId)
        {
            using (var ctx = new JobContext(_connectionString))
            {
                Job job = ctx.Jobs.Include(j => j.User).FirstOrDefault(j => j.Id == jobId);
                if (job == null)
                {
                    return new Job();
                }
                job.UserId = userId;
                job.Status = Status.Taken;
                job.User = ctx.Users.First(u => u.Id == userId);
                ctx.SaveChanges();
                return job;
            }
        }

        public void CompleteJob(int id, int userId)
        {
            using (var ctx = new JobContext(_connectionString))
            {
                Job job = ctx.Jobs.FirstOrDefault(j => j.Id == id && j.UserId == userId);
                if (job == null)
                {
                    return;
                }
                job.Status = Status.Complete;
                ctx.SaveChanges();
            }
        }
        
    }
}
