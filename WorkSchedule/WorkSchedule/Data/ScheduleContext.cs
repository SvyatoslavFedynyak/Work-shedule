using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkSchedule.Models;

namespace WorkSchedule.Data
{
    public class ScheduleContext : DbContext
    {
        public ScheduleContext(DbContextOptions<ScheduleContext> options) : base(options)
        {
        }

        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<TaskLine> TaskLines { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<WorkerType> WorkerTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Schedule>().
                HasKey(c => new { c.TaskID });
            modelBuilder.Entity<TaskLine>()
                .HasKey(c => new { c.TaskLineID });
            modelBuilder.Entity<Notification>().
                HasKey(c => new { c.NotificationID });
            modelBuilder.Entity<WorkerType>()
                .HasKey(c => new { c.WorkerTypeID });
        }


    }
}
