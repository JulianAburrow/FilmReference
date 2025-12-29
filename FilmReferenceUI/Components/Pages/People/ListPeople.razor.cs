namespace FilmReferenceUI.Components.Pages.People;

public partial class ListPeople
{
    [Parameter] public string Role { get; set; } = string.Empty;

    [Parameter] public string SelectedFilter { get; set; } = "A";

    protected List<PersonModel> PersonModels { get; set; } = null!;

    protected override async Task OnParametersSetAsync()
    {
        await FilterByInitial(SelectedFilter);
    }

    private bool _isLoading;

    private async Task FilterByInitial(string initial)
    {
        if (_isLoading)
        {
            return;
        }

        string header;

        try
        {
            _isLoading = true;

            switch (Enum.Parse<RoleEnum>(Role, true))
            {
                case RoleEnum.CastMembers:
                    header = "Cast Members";
                    PersonModels = await PersonHandler.GetCastMembersAsync(initial);
                    break;

                case RoleEnum.Directors:
                    header = "Directors";
                    PersonModels = await PersonHandler.GetDirectorsAsync(initial);
                    break;

                default:
                    header = "Error - no role selected";
                    PersonModels = [];
                    break;
            }

            SelectedFilter = initial;
            MainLayout.SetHeaderValue(header);
            StateHasChanged();
        }
        finally
        {
            _isLoading = false;
        }
    }
}

