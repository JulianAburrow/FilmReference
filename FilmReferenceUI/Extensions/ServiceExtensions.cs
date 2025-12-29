namespace FilmReferenceUI.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureSqlConnections(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<FilmReferenceContext>(
            options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("FilmReferenceConnectionString")));

    public static void AddDependencies(this IServiceCollection services)
    {
        services.AddScoped<IFilmHandler, FilmHandler>();
        services.AddScoped<IGenreHandler, GenreHandler>();
        services.AddScoped<IPersonHandler, PersonHandler>();
        services.AddScoped<IStudioHandler, StudioHandler>();
        services.AddScoped<ISearchHandler, SearchHandler>();
    }
}
