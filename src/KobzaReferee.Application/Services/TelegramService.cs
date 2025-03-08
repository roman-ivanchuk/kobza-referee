using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KobzaReferee.Application.Services;

public sealed class TelegramService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<TelegramService> _logger;

    private readonly ITelegramUserRepository _telegramUserRepository;
    private readonly ITournamentStatisticsRepository _tournamentStatisticsRepository;
    private readonly StandingsService _standingsService;
    private readonly WordGuessService _wordGuessService;

    public TelegramService(
        ITelegramBotClient botClient,
        ILogger<TelegramService> logger,
        ITelegramUserRepository telegramUserRepository,
        ITournamentStatisticsRepository tournamentStatisticsRepository,
        StandingsService standingsService,
        WordGuessService wordGuessService)
    {
        _botClient = botClient;
        _logger = logger;

        _telegramUserRepository = telegramUserRepository;
        _tournamentStatisticsRepository = tournamentStatisticsRepository;
        _standingsService = standingsService;
        _wordGuessService = wordGuessService;
    }

    public async Task OnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        if (!IsValidKobzaResultMessageFormat(message)) return;

        try
        {
            var wordGuess = await _wordGuessService.SubmitAsync(message, cancellationToken);

            var tournamentStatistics = await _standingsService.GetOrCreateTournamentAsync(
                chatId: wordGuess.ChatId,
                tournamentDate: wordGuess.Date,
                cancellationToken: cancellationToken);

            var tournamentParticipantsIds = tournamentStatistics.Standings.Select(tps => tps.UserId)
                .Distinct()
                .ToList();

            // TODO: [FUTURE] Areas for improvement in the future if the number of users grows significantly.
            var users = await _telegramUserRepository.GetAllAsync(cancellationToken: cancellationToken);

            tournamentStatistics = await SendStandingsMessageAsync(
                message: message,
                tournamentStatistics: tournamentStatistics,
                users: users,
                cancellationToken: cancellationToken);

            if (tournamentStatistics.AllDailyWordGuessesSubmitted)
            {
                tournamentStatistics = await SendSummaryMessageAsync(
                    message: message,
                    currentTournament: tournamentStatistics,
                    users: users,
                    cancellationToken: cancellationToken);
            }
        }
        catch (Exception ex)
        {
            await _botClient.SendMessage(
                message.Chat,
                ex.Message,
                replyParameters: new ReplyParameters() { MessageId = message.MessageId },
                cancellationToken: cancellationToken);
        }
    }

    private bool IsValidKobzaResultMessageFormat(Message message)
    {
        return message.Type == MessageType.Text
               && !string.IsNullOrWhiteSpace(message.Text)
               && message.Text.Contains(WordGuessMessage.StartIdentifier)
               && message.Text.Contains(WordGuessMessage.EndIdentifier);
    }

    private async Task<TournamentStatistics> SendStandingsMessageAsync(
        Message message,
        TournamentStatistics tournamentStatistics,
        List<TelegramUser> users,
        CancellationToken cancellationToken)
    {
        var standingsMessageText = TelegramMessageBuilder.BuildStandingsMessage(users, tournamentStatistics);

        var standingsMessage = await _botClient.SendMessage(
            chatId: message.Chat,
            text: standingsMessageText,
            replyParameters: new ReplyParameters() { MessageId = message.MessageId },
            disableNotification: true,
            cancellationToken: cancellationToken);

        if (tournamentStatistics.StandingsChatMessageId.HasValue)
        {
            try
            {
                await _botClient.DeleteMessage(
                    chatId: message.Chat,
                    messageId: tournamentStatistics.StandingsChatMessageId.Value,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting an old standings message.");
            }
        }

        tournamentStatistics.StandingsChatMessageId = standingsMessage.MessageId;
        tournamentStatistics = await _tournamentStatisticsRepository.SetStandingsChatMessageIdAsync(
            value: tournamentStatistics,
            cancellationToken: cancellationToken);

        return tournamentStatistics;
    }

    private async Task<TournamentStatistics> SendSummaryMessageAsync(
        Message message,
        TournamentStatistics currentTournament,
        List<TelegramUser> users,
        CancellationToken cancellationToken)
    {
        var tournaments = await _tournamentStatisticsRepository.GetAllAsync(
            partitionKeyValue: currentTournament.ChatId,
            predicate: ts => ts.AllDailyWordGuessesSubmitted,
            cancellationToken: cancellationToken);

        var overallParticipantStatistics = StandingsCalculator.CalculateOverallStandings(tournaments);

        var summaryMessageText = TelegramMessageBuilder.BuildSummaryMessage(
            tournamentStatistics: currentTournament,
            overallParticipantStatistics: overallParticipantStatistics,
            users: users);

        var summaryMessage = await _botClient.SendMessage(
            chatId: message.Chat,
            text: summaryMessageText,
            replyParameters: new ReplyParameters() { MessageId = currentTournament.StandingsChatMessageId.GetValueOrDefault() },
            disableNotification: true,
            cancellationToken: cancellationToken);

        if (currentTournament.SummaryChatMessageId.HasValue)
        {
            try
            {
                await _botClient.DeleteMessage(
                    chatId: message.Chat,
                    messageId: currentTournament.SummaryChatMessageId.Value,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting an old summary message.");
            }
        }

        currentTournament.SummaryChatMessageId = summaryMessage.MessageId;
        currentTournament = await _tournamentStatisticsRepository.SetSummaryChatMessageIdAsync(
            value: currentTournament,
            cancellationToken: cancellationToken);

        return currentTournament;
    }
}
