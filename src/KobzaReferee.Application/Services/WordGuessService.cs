using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KobzaReferee.Application.Services;

public sealed class WordGuessService
{
    private readonly ILogger<WordGuessService> _logger;

    private readonly ITelegramChatRepository _telegramChatRepository;
    private readonly ITelegramUserRepository _telegramUserRepository;
    private readonly IWordGuessRepository _wordGuessRepository;

    public WordGuessService(
        ILogger<WordGuessService> logger,
        ITelegramChatRepository telegramChatRepository,
        ITelegramUserRepository telegramUserRepository,
        IWordGuessRepository wordGuessRepository)
    {
        _logger = logger;

        _telegramChatRepository = telegramChatRepository;
        _telegramUserRepository = telegramUserRepository;
        _wordGuessRepository = wordGuessRepository;
    }

    public async Task<WordGuess> SubmitAsync(
        Message message,
        CancellationToken cancellationToken)
    {
        if (!IsValidKobzaResultMessageFormat(message))
        {
            throw new ArgumentException("The received message does not conform to the expected Kobza result format. Please check the message format and try again.");
        }

        var wordGuess = WordGuessParser.Execute(message.Text!);
        if (wordGuess is null)
        {
            throw new FormatException("Failed to parse WordGuess from the Kobza result message. Ensure the message is in the correct Kobza result format.");
        }

        var telegramUser = await EnsureUserExistsInContextAsync(message, cancellationToken);
        if (telegramUser is null)
        {
            throw new InvalidOperationException("User identification failed.");
        }

        var telegramChat = await EnsureChatExistsInContextAsync(message, cancellationToken);
        if (telegramChat is null)
        {
            throw new InvalidOperationException("Chat identification failed.");
        }

        var submittedAt = message.ForwardDate ?? message.Date;
        if (submittedAt == default)
        {
            wordGuess.SubmittedAt = DateTime.UtcNow;

            _logger.LogError("Cannot extract sent time from Telegram message.");
        }

        wordGuess.SubmittedAt = submittedAt.ToUkrainianTimeZone();
        wordGuess.ChatId = telegramChat.Id;
        wordGuess.UserId = telegramUser.Id;


        var wordGuesses = await _wordGuessRepository.GetAllAndMapAsync(
            selector: f => f.Id,
            predicate: f => f.UserId == wordGuess.UserId && f.Date == wordGuess.Date,
            cancellationToken: cancellationToken);

        if (wordGuesses.Any())
        {
            throw new InvalidOperationException("A Kobza result for the specified date and user already exists.");
        }

        var result = await _wordGuessRepository.CreateAsync(wordGuess, cancellationToken);

        return result;
    }

    private bool IsValidKobzaResultMessageFormat(Message message)
    {
        return message.Type == MessageType.Text
               && !string.IsNullOrWhiteSpace(message.Text)
               && message.Text.Contains(WordGuessMessage.StartIdentifier)
               && message.Text.Contains(WordGuessMessage.EndIdentifier);
    }

    private async Task<TelegramUser?> EnsureUserExistsInContextAsync(Message message, CancellationToken cancellationToken)
    {
        var messageUser = message.ForwardFrom ?? message.From;
        if (messageUser is null)
        {
            throw new ArgumentException("Cannot extract user from Telegram message.", nameof(message));
        }

        var telegramUser = await _telegramUserRepository.GetByIdAsync(messageUser.Id.ToString(), cancellationToken: cancellationToken);
        if (telegramUser is null)
        {
            telegramUser = new()
            {
                Id = messageUser.Id.ToString(),
                FirstName = messageUser.FirstName,
                LastName = messageUser.LastName,
                Username = messageUser.Username,
                IsBot = messageUser.IsBot,
                LanguageCode = messageUser.LanguageCode
            };

            await _telegramUserRepository.CreateAsync(telegramUser, cancellationToken);
        }

        return telegramUser;
    }

    private async Task<TelegramChat?> EnsureChatExistsInContextAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Chat is null)
        {
            throw new ArgumentException("Cannot extract chat from Telegram message.", nameof(message));
        }

        var telegramChat = await _telegramChatRepository.GetByIdAsync(message.Chat.Id.ToString(), cancellationToken: cancellationToken);
        if (telegramChat is null)
        {
            telegramChat = new()
            {
                Id = message.Chat.Id.ToString(),
                FirstName = message.Chat.FirstName,
                LastName = message.Chat.LastName,
                Username = message.Chat.Username,
                IsForum = message.Chat.IsForum,
                Title = message.Chat.Title,
                Type = Domain.Common.Constants.ChatType.Validate(message.Chat.Type.ToString())
            };

            await _telegramChatRepository.CreateAsync(telegramChat, cancellationToken);
        }

        return telegramChat;
    }
}
