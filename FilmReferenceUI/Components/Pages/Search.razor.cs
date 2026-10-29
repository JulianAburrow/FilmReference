namespace FilmReferenceUI.Components.Pages;

public partial class Search
{
    private SearchModel SearchModel = new();

    private bool SubmitClicked;

    private List<FilmModel> FilmsFound = [];

    private List<GenreModel> GenresFound = [];

    private List<PersonModel> PeopleFound = [];

    private List<StudioModel> StudiosFound = [];

    private MudTextField<string>? SearchTextBox;

    protected override async Task OnInitializedAsync()
    {
        if (SearchState.LastSearchValue > SharedValues.PleaseSelectValue)
        {
            SearchModel.SearchType = SearchState.LastSearchValue;
            SearchModel.SearchText = SearchState.LastSearchText;
            SearchModel.IncludeDescriptionAndNotes = SearchState.IncludeDescriptionAndNotes;
            await DoSearch();
        }
        MainLayout.SetHeaderValue("Search");
    }

    private async Task DoSearch()
    {
        SubmitClicked = true;
        SearchState.LastSearchValue = SearchModel.SearchType;
        SearchState.LastSearchText = SearchModel.SearchText;
        SearchState.IncludeDescriptionAndNotes = SearchModel.IncludeDescriptionAndNotes;

        switch ((SearchTypeEnum)SearchModel.SearchType)
        {
            case SearchTypeEnum.Film:
                FilmsFound = await SearchHandler.SearchFilmsAsync(SearchModel.SearchText, SearchModel.IncludeDescriptionAndNotes);
                break;
            case SearchTypeEnum.Genre:
                GenresFound = await SearchHandler.SearchGenresAsync(SearchModel.SearchText, SearchModel.IncludeDescriptionAndNotes);
                break;
            case SearchTypeEnum.Person:
                PeopleFound = await SearchHandler.SearchPeopleAsync(SearchModel.SearchText, SearchModel.IncludeDescriptionAndNotes);
                break;
            case SearchTypeEnum.Studio:
                StudiosFound = await SearchHandler.SearchStudiosAsync(SearchModel.SearchText, SearchModel.IncludeDescriptionAndNotes);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(SearchModel.SearchType), SearchModel.SearchType, "Invalid search type");
        }
    }

    private void ClearSearch()
    {
        SubmitClicked = false;
        SearchModel.SearchType = SharedValues.PleaseSelectValue;
        SearchState.LastSearchValue = SharedValues.PleaseSelectValue;
        SearchModel.SearchText = string.Empty;
        SearchState.LastSearchText = string.Empty;
        SearchModel.IncludeDescriptionAndNotes = false;
        SearchState.IncludeDescriptionAndNotes = false;
    }

    private async Task ClearSearchText()
    {
        SearchModel.SearchText = string.Empty;
        SearchModel.IncludeDescriptionAndNotes = false;
        if (SearchTextBox is not null)
        {
            await SearchTextBox.FocusAsync();
        }
    }
}