using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchedulingReminders.Models;
using SchedulingReminders.Repository;
using SchedulingReminders.Services;

namespace SchedulingReminders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemindersController : ControllerBase
    {
        private readonly IReminderRepository _repository;
        private readonly IEmailService _emailService;
        private readonly ITelegramService _telegramService;

        public RemindersController(IReminderRepository repository, IEmailService emailService, ITelegramService telegramService)
        {
            _repository = repository;
            _emailService = emailService;
            _telegramService = telegramService;
        }

        // GET: api/Reminders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reminder>>> GetReminders()
        {
            return Ok(await _repository.GetAllRemindersAsync());
        }

        // GET: api/Reminders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reminder>> GetReminder(int id)
        {
            var reminder = await _repository.GetReminderByIdAsync(id);

            if (reminder == null)
            {
                return NotFound();
            }

            return Ok(reminder);
        }


        // POST: api/Reminders
        [HttpPost]
        public async Task<ActionResult<Reminder>> PostReminder(Reminder reminder)
        {
            if (reminder.SendAt <= DateTime.UtcNow)
            {
                return BadRequest("SendAt parameter must be greater than the current date and time.");
            }

            // Check if the reminder method is "email"
            if (reminder.Method.ToLower() == "email")
            {
                // Validate the "To" parameter
                if (string.IsNullOrWhiteSpace(reminder.To) || !IsValidEmail(reminder.To))
                {
                    return BadRequest("The 'To' parameter should be a valid email address.");
                }

                // Send the email
                string subject = "Your Scheduled Reminder";
                await _emailService.SendEmailAsync(reminder.To, subject, reminder.Content);
                return Ok("Message sent to Email successfully.");


            }
            else if(reminder.Method.ToLower() == "telegram")
            {
                    try
                    {
                        long chatId = long.Parse(reminder.To);
                        await _telegramService.SendMessageAsync(chatId, reminder.Content);
                    await _repository.AddReminderAsync(reminder);
                    return Ok("Message sent to Telegram successfully.");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest($"Error sending message to Telegram: {ex.Message}");
                    }
            }

            await _repository.AddReminderAsync(reminder);

            return CreatedAtAction(nameof(GetReminder), new { id = reminder.Id }, reminder);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // DELETE: api/Reminders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReminder(int id)
        {
            await _repository.DeleteReminderAsync(id);
            return NoContent();
        }

        // DELETE: api/Reminders/bulk-delete
        [HttpDelete("bulk-delete")]
        public async Task<IActionResult> BulkDeleteReminders([FromBody] List<int> ids)
        {
            var deletedIds = await _repository.DeleteRemindersAsync(ids);
            return Ok(new { DeletedIds = deletedIds });
        }
    }


}
