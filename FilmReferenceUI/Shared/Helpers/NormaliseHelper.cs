namespace FilmReferenceUI.Shared.Helpers;

public static class NormaliseHelper
{
    public static string Normalised(string value)
        => value.ToLower().Replace(" ", "");
}
