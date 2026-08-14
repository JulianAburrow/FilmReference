namespace FilmReferenceUI.Extensions;

public static class IntExtensions
{
    public static string ToDayOrdinal(this int day)
    {
        if (day < 1 || day > 31)
            throw new ArgumentOutOfRangeException(nameof(day), "Day must be between 1 and 31.");

        int rem100 = day % 100;
        if (rem100 is 11 or 12 or 13)
            return $"{day}th";

        return (day % 10) switch
        {
            1 => $"{day}st",
            2 => $"{day}nd",
            3 => $"{day}rd",
            _ => $"{day}th"
        };
    }
}