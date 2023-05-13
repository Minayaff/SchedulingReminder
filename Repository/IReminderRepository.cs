using SchedulingReminders.Models;

namespace SchedulingReminders.Repository
{
    public interface IReminderRepository
    {
        Task<IEnumerable<Reminder>> GetAllRemindersAsync();
        Task<Reminder> GetReminderByIdAsync(int id);
        Task<Reminder> AddReminderAsync(Reminder reminder);
        Task UpdateReminderAsync(Reminder reminder);
        Task DeleteReminderAsync(int id);
        Task<IEnumerable<int>> DeleteRemindersAsync(IEnumerable<int> ids);
    }
}
