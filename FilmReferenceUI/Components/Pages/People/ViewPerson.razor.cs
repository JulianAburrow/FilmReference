namespace FilmReferenceUI.Components.Pages.People;

public partial class ViewPerson
{
    private List<FilmModel> FilmsStarredIn { get; set; } = [];

    private List<FilmModel> FilmsDirected { get; set; } = [];

    private enum FilmListToSortEnum
    {
        FilmsDirected = 0,
        FilmsStarredIn = 1,
    };

    protected override async Task OnInitializedAsync()
    {
        PersonModel = await PersonHandler.GetPersonAsync(PersonId);

        if (PersonModel.PersonId == 0)
        {
            MainLayout.SetHeaderValue(PersonNotFoundMessage);
            _entityNotFound = true;
            return;
        }

        PreventDeleting = PersonModel.FilmPerson.Count != 0 || PersonModel.Films.Count != 0;
        MainLayout.SetHeaderValue($"{PersonModel.FirstName} {PersonModel.LastName}");
        var isFavourite = await FavouriteHandler.IsFavouriteAsync((int)FavouriteEntityEnum.Person, PersonId);
        MainLayout.ConfigureFavouriteButton(FavouriteEntityEnum.Person, PersonId, isFavourite);

        _isLoaded = true;

        if (PersonModel.FilmPerson is null)
        {
            return;
        }
        foreach (var filmPerson in PersonModel.FilmPerson)
        {
            FilmsStarredIn.Add(filmPerson.Film);
        }
        FilmsDirected = PersonModel.Films
            .OrderBy(f => f.Name)
            .ToList();
        NextSortDirection = SortDirection.Descending;
    }

    private void DoNavigation(RoleEnum role)
    {
        if (role == RoleEnum.CastMembers)
        {
            NavigationManager.NavigateTo($"/people/listpeople/{RoleEnum.CastMembers}");
        }
        if (role == RoleEnum.Directors)
        {
            NavigationManager.NavigateTo($"/people/listpeople/{RoleEnum.Directors}");
        }
    }

    private void ResortList(FilmListToSortEnum listToSort)
    {
        switch (NextSortDirection)
        {
            case SortDirection.Ascending:
                if (listToSort == FilmListToSortEnum.FilmsStarredIn)
                {
                    FilmsStarredIn = [.. FilmsStarredIn.OrderBy(f => f.Name)];
                }
                else
                {
                    FilmsDirected = [.. FilmsDirected.OrderBy(f => f.Name)];
                }

                NextSortDirection = SortDirection.Descending;
                break;
        case SortDirection.Descending:
            if (listToSort == FilmListToSortEnum.FilmsStarredIn)
            {
                FilmsStarredIn = [.. FilmsStarredIn.OrderByDescending(f => f.Name)];
            }
            else
            {
                FilmsDirected = [.. FilmsDirected.OrderByDescending(f => f.Name)];
            }

            NextSortDirection = SortDirection.Ascending;
            break;
        default:
            if (listToSort == FilmListToSortEnum.FilmsStarredIn)
            {
                FilmsStarredIn = [.. FilmsStarredIn.OrderBy(f => f.Name)];
            }
            else
            {
                FilmsDirected = [.. FilmsDirected.OrderBy(f => f.Name)];
            }

            NextSortDirection = SortDirection.Descending;
            break;
        }
    }
}