namespace FilmReferenceUI.Shared.Services;

public class SearchState
{
    public int LastSearchValue { get; set; } = SharedValues.SharedValues.PleaseSelectValue;

    public string LastSearchText { get; set; } = string.Empty;
}
