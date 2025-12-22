using static MudBlazor.Colors;

namespace FilmReferenceUI.Components.Pages.People;

public partial class ListPeople
{
    [Parameter] public string Role { get; set; } = string.Empty;

    [Parameter] public string SelectedFilter { get; set; } = "A";

    protected List<PersonModel> PersonModels { get; set; } = null!;

    private bool Loading;

    protected override async Task OnParametersSetAsync()
    {
        if (Loading)
        {
            return;
        }

        Loading = true;

        _ = Enum.TryParse<RoleEnum>(Role, true, out var roleEnum);

        var (header, people) = roleEnum switch
        {
            RoleEnum.CastMembers => ("Cast Members", await PersonHandler.GetCastMembersAsync(SelectedFilter)),
            RoleEnum.Directors => ("Directors", await PersonHandler.GetDirectorsAsync(SelectedFilter)),
            _ => ("Error - no role selected", []),
        };

        MainLayout.SetHeaderValue(header);
        PersonModels = people;
    }

    private void Navigate(string letter)
    {
        NavigationManager.NavigateTo($"people/listpeople/{Role.ToLower()}/{letter.ToLower()}",forceLoad: true);
    }
}

