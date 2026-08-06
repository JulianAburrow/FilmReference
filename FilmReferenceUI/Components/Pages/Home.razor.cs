namespace FilmReferenceUI.Components.Pages;

public partial class Home
{
    private SearchModel SearchModel = new();

    private List<FilmModel> FilmsFound = [];

    private List<GenreModel> GenresFound = [];

    private List<PersonModel> PeopleFound = [];

    private bool TodayButtonVisible => SelectedDay != DateTime.Today.Day || SelectedMonth != DateTime.Today.Month;

    private bool IsLoadingBirthdays = true;

    private bool ShowBirthdays = true;

    private int SelectedDay = DateTime.Today.Day;

    private int SelectedMonth = DateTime.Today.Month;

    List<PersonModel> BirthdayPeople = [];

    private bool BirthdaysToShow;

    private FeaturedPersonModel? FeaturedPerson = null!;

    private bool FeaturedPersonToShow;

    private bool FirstLoad = true;

    private List<StudioModel> StudiosFound = [];

    private bool SubmitClicked;

    private MudTextField<string>? SearchTextBox;

    protected override async Task OnInitializedAsync()
    {
        if (SearchState.LastSearchValue > SharedValues.PleaseSelectValue)
        {
            SearchModel.SearchType = SearchState.LastSearchValue;
        }
        MainLayout.SetHeaderValue("Home / Search");  
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;

        await GetBirthdayPeople();

        FeaturedPerson = await PersonHandler.GetFeaturedPersonAsync();
        FeaturedPersonToShow = FeaturedPerson is not null && FeaturedPerson.PersonId != 0;

        // NEW LOGIC: if no birthdays on first load, show featured person instead
        if (FirstLoad && !BirthdaysToShow)
        {
            ShowBirthdays = false;
        }

        FirstLoad = false;

        StateHasChanged();
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

    private void SwitchBirthdaysFeaturedPersonDisplay()
    {
        ShowBirthdays = !ShowBirthdays;
    }

    private int GetDaysInMonth(int month)
    {
        return month switch
        {
            1 => 31,
            2 => 29, // year-agnostic February
            3 => 31,
            4 => 30,
            5 => 31,
            6 => 30,
            7 => 31,
            8 => 31,
            9 => 30,
            10 => 31,
            11 => 30,
            12 => 31,
            _ => 31
        };
    }

    private async Task GetBirthdayPeople()
    {
        IsLoadingBirthdays = true;

        BirthdayPeople = await PersonHandler.GetBirthdaysForDateAsync(SelectedDay, SelectedMonth);
        BirthdaysToShow = BirthdayPeople.Count > 0;
        var monthWord = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(SelectedMonth);
        if (BirthdayPeople.Count == 0)
        {
            Snackbar.Add(
                $"No birthdays found for {SelectedDay} {monthWord}.",
                Severity.Warning);
        }
        else
        {
            Snackbar.Add(
                $"{BirthdayPeople.Count} birthday{(BirthdayPeople.Count != 1 ? "s" : "")} found for {SelectedDay} {monthWord}.",
                Severity.Info);
        }
        IsLoadingBirthdays = false;
    }

    private async Task SelectedDateChanged()
    {
        // Clamp the selected day to the max valid day for the new month
        int maxDays = GetDaysInMonth(SelectedMonth);

        if (SelectedDay > maxDays)
            SelectedDay = maxDays;

        await GetBirthdayPeople();
        BirthdaysToShow = BirthdayPeople.Count > 0;

        StateHasChanged();
    }

    private async Task SetSelectedDatesToToday()
    {
        SelectedDay = DateTime.Today.Day;
        SelectedMonth = DateTime.Today.Month;

        await SelectedDateChanged();
    }
}
