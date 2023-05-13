using Microsoft.EntityFrameworkCore;
using SchedulingReminders.Models;

namespace SchedulingReminders.DAL
{
    public class ReminderDbContext : DbContext
    {
        public ReminderDbContext(DbContextOptions<ReminderDbContext> options) : base(options) { }

        public DbSet<Reminder> Reminders { get; set; }
    }
}
