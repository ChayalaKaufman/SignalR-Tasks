using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using RealTimeTasks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealTimeTasks.Web
{
    public class UserJobAction
    {
        public int UserId { get; set; }
        public int JobId { get; set; }
    }
    public class JobsHub : Hub
    { 
        private string _connectionString;
        private JobRepository _db;
        private UsersRepository _userDb;

        public JobsHub(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _db = new JobRepository(_connectionString);
            _userDb = new UsersRepository(_connectionString);
        }
       
        public void AddJob(string name)
        {
            var job = new Job
            {
                Name = name,
                Status = Status.Incomplete
            };
            job.Id = _db.AddJob(job);

            Clients.All.SendAsync("NewJob", job);
        }

        public void TakeJob(UserJobAction uja)
        {
           Job job = _db.AssignJob(uja.JobId, uja.UserId);
            
           Clients.Others.SendAsync("JobTaken", job);
            
           Clients.Caller.SendAsync("MyJob", job);
        }

        public void CompleteJob(UserJobAction uja)
        { 
            //test here if userId is logged in or return
            _db.CompleteJob(uja.JobId, uja.UserId);

            Clients.All.SendAsync("JobCompleted", uja.JobId);
        }

        public void Logout()
        {
            //log them out - how??
        }
    }
}
