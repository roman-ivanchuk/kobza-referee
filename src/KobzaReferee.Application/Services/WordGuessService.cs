using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KobzaReferee.Application.Services;

public sealed class WordGuessService
{
    private readonly ILogger<WordGuessService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public WordGuessService(ILogger<WordGuessService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
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


        var wordGuesses = await _unitOfWork.WordGuesses.GetAllAndMapAsync(
            selector: f => f.Id,
            predicate: f => f.UserId == wordGuess.UserId && f.Date == wordGuess.Date,
            cancellationToken: cancellationToken);

        if (wordGuesses.Any())
        {
            throw new InvalidOperationException("A Kobza result for the specified date and user already exists.");
        }

        var result = await _unitOfWork.WordGuesses.CreateAsync(wordGuess, cancellationToken);

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

        var telegramUser = await _unitOfWork.TelegramUsers.GetByIdAsync(messageUser.Id.ToString(), cancellationToken: cancellationToken);
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

            await _unitOfWork.TelegramUsers.CreateAsync(telegramUser, cancellationToken);
        }

        return telegramUser;
    }

    private async Task<TelegramChat?> EnsureChatExistsInContextAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Chat is null)
        {
            throw new ArgumentException("Cannot extract chat from Telegram message.", nameof(message));
        }

        var telegramChat = await _unitOfWork.TelegramChats.GetByIdAsync(message.Chat.Id.ToString(), cancellationToken: cancellationToken);
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

            await _unitOfWork.TelegramChats.CreateAsync(telegramChat, cancellationToken);
        }

        return telegramChat;
    }
}
