using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Models
{
    public class RUNContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<File> Files { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
