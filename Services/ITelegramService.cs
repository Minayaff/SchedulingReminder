using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SchedulingReminders.Services
{

    public interface ITelegramService
    {
        Task SendMessageAsync(long chatId, string message);
        Task<Message> ProcessUpdateAsync(Update update);
    }
    public class TelegramService : ITelegramService
    {
        private readonly ITelegramBotClient _telegramBotClient;

        public TelegramService(IConfiguration configuration)
        {
            string botToken = configuration["TelegramSettings:BotToken"];
            _telegramBotClient = new TelegramBotClient(botToken);
        }

        public async Task SendMessageAsync(long chatId, string message)
        {
            await _telegramBotClient.SendTextMessageAsync(chatId, message);
        }

        public async Task<Message> ProcessUpdateAsync(Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                if (message.Type == MessageType.Text)
                {
                    await SendMessageAsync(message.Chat.Id, $"You said: {message.Text}");
                }
            }

            return null;
        }
    }

}
