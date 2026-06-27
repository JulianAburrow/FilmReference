using FilmReferenceDataAccess.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.CompilerServices;

namespace FilmReferenceUI.Shared.Components;

public partial class ListFavouritesComponent
{
    [Parameter] public List<FavouriteDisplayModel> FavouriteDisplayModels { get; set; } = null!;

    [Parameter] public EventCallback OnRemoved { get; set; }

    private Dictionary<int, List<FavouriteDisplayModel>> _groupedFavourites = [];
    
    private Dictionary<int, SortDirection> _sortDirections = [];

    protected override void OnParametersSet()
    {
        _groupedFavourites = FavouriteDisplayModels
            .GroupBy(f => f.EntityTypeId)
            .ToDictionary(
                g => g.Key,
                g => g.ToList()
            );

        _sortDirections = _groupedFavourites.Keys.ToDictionary(k => k, k => SortDirection.Descending);
    }

    private async Task HandleRemoved()
    {
        await OnRemoved.InvokeAsync();
    }

    private void ResortList(string entityTypeName)
    {
        var entityTypeId = (int)Enum.Parse<FavouriteEntityEnum>(entityTypeName);

        var currentDirection = _sortDirections[entityTypeId];
        var list = _groupedFavourites[entityTypeId];

        if (currentDirection == SortDirection.Ascending)
        {
            _groupedFavourites[entityTypeId] = list
                .OrderBy(f => f.EntityName)
                .ToList();

            _sortDirections[entityTypeId] = SortDirection.Descending;
        }
        else
        {
            _groupedFavourites[entityTypeId] = list
                .OrderByDescending(f => f.EntityName)
                .ToList();

            _sortDirections[entityTypeId] = SortDirection.Ascending;
        }
    }
}