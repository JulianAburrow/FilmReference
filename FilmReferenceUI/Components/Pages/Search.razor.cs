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
        }
        MainLayout.SetHeaderValue("Search");
    }

    private async Task DoSearch()
    {
        SubmitClicked = true;
        SearchState.LastSearchValue = SearchModel.SearchType;

        switch ((SearchTypeEnum)SearchModel.SearchType)
        {
            case SearchTypeEnum.Film:
                FilmsFound = await SearchHandler.SearchFilmsAsync(SearchModel.SearchText);
                break;
            case SearchTypeEnum.Genre:
                GenresFound = await SearchHandler.SearchGenresAsync(SearchModel.SearchText);
                break;
            case SearchTypeEnum.Person:
                PeopleFound = await SearchHandler.SearchPeopleAsync(SearchModel.SearchText);
                break;
            case SearchTypeEnum.Studio:
                StudiosFound = await SearchHandler.SearchStudiosAsync(SearchModel.SearchText);
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
    }

    private async Task ClearSearchText()
    {
        SearchModel.SearchText = string.Empty;
        if (SearchTextBox is not null)
        {
            await SearchTextBox.FocusAsync();
        }
    }
}