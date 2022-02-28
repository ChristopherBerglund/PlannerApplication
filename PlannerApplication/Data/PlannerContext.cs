using Microsoft.EntityFrameworkCore;
using PlannerApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApplication.Data
{
    public class PlannerContext : DbContext
    {
        public PlannerContext(DbContextOptions options)
            : base(options) { }
    

        public DbSet<activity> activity { get; set; }
        public DbSet<newactivity> newactivity { get; set; }
        public DbSet<planneruser> planneruser { get; set; }
        public DbSet<participant> participant { get; set; }
        public DbSet<picture> picture { get; set; }
        //public DbSet<imagemodel> imagemodel { get; set; }



        
    }
}


