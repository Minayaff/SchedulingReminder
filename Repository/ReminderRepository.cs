using Microsoft.EntityFrameworkCore;
using SchedulingReminders.DAL;
using SchedulingReminders.Models;

namespace SchedulingReminders.Repository
{
    public class ReminderRepository : IReminderRepository
    {
        private readonly ReminderDbContext _context;

        public ReminderRepository(ReminderDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reminder>> GetAllRemindersAsync()
        {
            var data= await _context.Reminders.ToListAsync();
            return data;
        }

        public async Task<Reminder> GetReminderByIdAsync(int id)
        {
            return await _context.Reminders.FindAsync(id);
        }

        public async Task<Reminder> AddReminderAsync(Reminder reminder)
        {
            _context.Reminders.Add(reminder);
            await _context.SaveChangesAsync();
            return reminder;
        }

        public async Task UpdateReminderAsync(Reminder reminder)
        {
            _context.Entry(reminder).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReminderAsync(int id)
        {
            var reminder = await _context.Reminders.FindAsync(id);
            if (reminder != null)
            {
                _context.Reminders.Remove(reminder);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<int>> DeleteRemindersAsync(IEnumerable<int> ids)
        {
            var reminders = _context.Reminders.Where(r => ids.Contains(r.Id));
            if (reminders != null && reminders.Any())
            {
                _context.Reminders.RemoveRange(reminders);
                await _context.SaveChangesAsync();
                return reminders.Select(r => r.Id);
            }
            return new List<int>();
        }
    }

}
