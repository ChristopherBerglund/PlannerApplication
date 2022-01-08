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
        public PlannerContext(DbContextOptions<PlannerContext> options)
            : base(options) { }
    

        public DbSet<activity> activity { get; set; }
        public DbSet<forum> forum { get; set; }
        public DbSet<newactivity> newactivity { get; set; }
        //public DbSet<participant> participant { get; set; }
        public DbSet<user> user{ get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    try
        //    {
        //        optionsBuilder.UseMySql(_connectionstring, ServerVersion.AutoDetect(_connectionstring));
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //    }
        //}
    }
}
