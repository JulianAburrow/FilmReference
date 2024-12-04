namespace FilmReferenceDataAccess.Data;

public class FilmReferenceContext(DbContextOptions<FilmReferenceContext> options) : DbContext(options)
{
    public required DbSet<FilmModel> Films { get; set; }
    public required DbSet<FilmPersonModel> FilmPeople { get; set; }
    public required DbSet<GenreModel> Genres { get; set; }
    public required DbSet<PersonModel> People { get; set; }
    public required DbSet<StudioModel> Studios { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties()
            .Where(p => p.ClrType == typeof(string))))
        {
            property.SetIsUnicode(false);
        }

        builder.ApplyConfiguration(new FilmConfiguration());
        builder.ApplyConfiguration(new FilmPersonConfiguration());
        builder.ApplyConfiguration(new GenreConfiguration());
        builder.ApplyConfiguration(new PersonConfiguration());
        builder.ApplyConfiguration(new StudioConfiguration());
    }
}
