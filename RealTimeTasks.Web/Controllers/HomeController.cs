using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeTasks.Web.Models;
using RealTimeTasks.Data;
using Microsoft.Extensions.Configuration;

namespace RealTimeTasks.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString;

        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [Authorize]
        public IActionResult Index()
        {
            JobRepository db = new JobRepository(_connectionString);
            var jobs = db.GetJobs();
            UsersRepository users = new UsersRepository(_connectionString);
            var vm = new IndexViewModel
            {
                Jobs = jobs,
                User = users.GetByEmail(User.Identity.Name)
            };
            return View(vm);
        }
    }
}
