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
        services.AddTransient<IFilmHandler, FilmHandler>();
    }
}
