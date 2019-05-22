﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealTimeTasks.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public List<Job> Jobs { get; set; }
    }
}
