namespace KobzaReferee.Application.Dtos;

public class OverallStatisticsDto
{
    /// <summary>
    /// Standings statistics for each participant.
    /// </summary>
    public List<OverallParticipantStatisticsDto> Standings { get; set; } = new();
}
