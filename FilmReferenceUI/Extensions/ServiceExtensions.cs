namespace FilmReferenceUI.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureSqlConnections(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContextFactory<FilmReferenceContext>(
            options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("FilmReferenceConnectionString")));

    public static void AddDependencies(this IServiceCollection services)
    {
        services.AddTransient<IFavouriteHandler, FavouriteHandler>();
        services.AddTransient<IFilmHandler, FilmHandler>();
        services.AddTransient<IGenreHandler, GenreHandler>();
        services.AddTransient<INationalityHandler, NationalityHandler>();
        services.AddTransient<IPersonHandler, PersonHandler>();
        services.AddTransient<IStudioHandler, StudioHandler>();
        services.AddTransient<ISearchHandler, SearchHandler>();
        services.AddScoped<SearchState>();
    }
}
