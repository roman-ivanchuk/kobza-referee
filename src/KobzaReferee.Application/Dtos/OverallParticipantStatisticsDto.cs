namespace KobzaReferee.Application.Dtos;

public class OverallParticipantStatisticsDto
{
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Total points for the participant's standing.
    /// </summary>
    public int TotalPointsForStanding { get; set; }

    /// <summary>
    /// Count of tournaments participated in.
    /// </summary>
    public int TournamentCount { get; set; }

    /// <summary>
    /// Total number of wins.
    /// </summary>
    public int TotalWins { get; set; }

    /// <summary>
    /// Average points for the participant's standing.
    /// </summary>
    public double AveragePointsForStanding { get; set; }
}
