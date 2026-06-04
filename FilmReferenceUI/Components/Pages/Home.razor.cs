using System.Timers;

namespace FilmReferenceUI.Components.Pages;

public partial class Home
{
    private SearchModel SearchModel = new();

    private List<FilmModel> FilmsFound = [];

    private List<GenreModel> GenresFound = [];

    private List<PersonModel> PeopleFound = [];

    private RandomPersonModel? RandomPerson = null!;

    private List<StudioModel> StudiosFound = [];

    private bool SubmitClicked;

    private MudTextField<string>? SearchTextBox;

    protected override async Task OnInitializedAsync()
    {
        SearchModel.SearchType = SharedValues.PleaseSelectValue;
        MainLayout.SetHeaderValue("Home / Search");        
        RandomPerson = await PersonHandler.GetRandomPersonAsync();
    }

    private async Task DoSearch()
    {
        SubmitClicked = true;

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
