namespace KobzaReferee.Application.Services;

public sealed class StandingsService
{
    private readonly ILogger<StandingsService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public StandingsService(ILogger<StandingsService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<TournamentStatistics> GetOrCreateTournamentAsync(
        string chatId,
        DateTime tournamentDate,
        CancellationToken cancellationToken)
    {
        var tournament = await TryGetTournamentAsync(chatId, tournamentDate, cancellationToken);

        if (tournament is null)
        {
            // Place where it can be enhanced in future and define tournaments duration
            // based on chat preferences, for example weekly, biweekly, monthly, etc.
            var (startDate, endDate) = tournamentDate.GetStartAndEndOfWeek();

            tournament = new()
            {
                Id = Guid.NewGuid().ToString(),
                ChatId = chatId,
                StartDate = startDate,
                EndDate = endDate,
                AllDailyWordGuessesSubmitted = false
            };
        }

        // TODO: Fix in future: too many data need to make distinct on db side
        var chatUserIds = await _unitOfWork.WordGuesses.GetAllAndMapAsync(
            selector: f => f.UserId,
            predicate: f => f.ChatId == chatId,
            cancellationToken: cancellationToken);

        chatUserIds = chatUserIds.Distinct().ToList();

        var chatUsersWordGuesses = await _unitOfWork.WordGuesses.GetAllAsync(
            predicate: f => chatUserIds.Contains(f.UserId) && f.Date >= tournament.StartDate && f.Date <= tournament.EndDate,
            cancellationToken: cancellationToken);

        tournament.Standings = StandingsCalculator.CalculateTournamentStandings(chatUsersWordGuesses);

        var result = await _unitOfWork.TournamentStatistics.UpsertAsync(
            value: tournament,
            cancellationToken: cancellationToken);

        return result;
    }

    private async Task<TournamentStatistics?> TryGetTournamentAsync(
        string chatId,
        DateTime tournamentDate,
        CancellationToken cancellationToken)
    {
        var tournaments = await _unitOfWork.TournamentStatistics.GetAllAsync(
            partitionKeyValue: chatId,
            predicate: tournament => tournamentDate >= tournament.StartDate && tournamentDate <= tournament.EndDate && tournament.ChatId == chatId,
            cancellationToken: cancellationToken);

        if (tournaments.Count > 1)
        {
            _logger.LogError($"'TryGetTournamentAsync' found more than 1 item for parameters 'chatId' - {chatId} and 'tournamentDate' - {tournamentDate.ToShortDateString()}");
        }

        return tournaments.FirstOrDefault();
    }
}
