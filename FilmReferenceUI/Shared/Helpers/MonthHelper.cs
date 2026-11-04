namespace FilmReferenceUI.Shared.Helpers;

public class MonthHelper
{
    public static string GetMonthName(int month)
    {
        return new DateTime(2000, month, 1).ToString("MMMM");
    }
}
