namespace KobzaReferee.Application.Helpers;

internal static class DateTimeHelper
{
    public static (DateTime startOfWeek, DateTime endOfWeek) GetStartAndEndOfWeek(this DateTime date)
    {
        var offset = date.DayOfWeek switch
        {
            DayOfWeek.Monday => 0,
            DayOfWeek.Tuesday => 1,
            DayOfWeek.Wednesday => 2,
            DayOfWeek.Thursday => 3,
            DayOfWeek.Friday => 4,
            DayOfWeek.Saturday => 5,
            DayOfWeek.Sunday => 6,
            _ => throw new ArgumentException($"Invalid DayOfWeek: {date.DayOfWeek}. Expected a day of the week.")
        };

        DateTime startOfWeek = date.AddDays(-offset);
        DateTime endOfWeek = startOfWeek.AddDays(6);

        return (startOfWeek, endOfWeek);
    }

    public static DateTime ToUkrainianTimeZone(this DateTime date)
    {
        var ukrainianZone = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");
        var ukrainianDateTime = TimeZoneInfo.ConvertTimeFromUtc(date, ukrainianZone);

        return ukrainianDateTime;
    }
}
