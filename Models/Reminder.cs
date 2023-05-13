namespace SchedulingReminders.Models
{
    public class Reminder
    {
        public int Id { get; set; }
        public string To { get; set; }
        public string Content { get; set; }
        public DateTime SendAt { get; set; }
        public string Method { get; set; }
    }
}
