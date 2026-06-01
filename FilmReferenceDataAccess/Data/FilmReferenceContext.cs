namespace FilmReferenceDataAccess.Data;

public class FilmReferenceContext(DbContextOptions<FilmReferenceContext> options) : DbContext(options)
{
    public DbSet<FavouriteModel> Favourites { get; set; }
    public DbSet<FilmModel> Films { get; set; }
    public DbSet<FilmPersonModel> FilmPeople { get; set; }
    public DbSet<GenreModel> Genres { get; set; }
    public DbSet<PersonModel> People { get; set; }
    public DbSet<StudioModel> Studios { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties()
            .Where(p => p.ClrType == typeof(string))))
        {
            property.SetIsUnicode(false);
        }

        builder.ApplyConfiguration(new FavouriteConfiguration());
        builder.ApplyConfiguration(new FilmConfiguration());
        builder.ApplyConfiguration(new FilmPersonConfiguration());
        builder.ApplyConfiguration(new GenreConfiguration());
        builder.ApplyConfiguration(new PersonConfiguration());
        builder.ApplyConfiguration(new StudioConfiguration());
    }
}
