using System;

namespace FilmReferenceDataAccess.Data;

public class FilmReferenceContext : DbContext
{
    public FilmReferenceContext(DbContextOptions<FilmReferenceContext> options) : base(options) { }

    public DbSet<FilmModel> Films { get; set; }
    public DbSet<FilmPersonModel> FilmPeople { get; set; }
    public DbSet<GenreModel> Genres { get; set; }
    public DbSet<PersonModel> People { get; set; }
    public DbSet<StudioModel> Studios { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new FilmConfiguration());
        builder.ApplyConfiguration(new FilmPersonConfiguration());
        builder.ApplyConfiguration(new GenreConfiguration());
        builder.ApplyConfiguration(new PersonConfiguration());
        builder.ApplyConfiguration(new StudioConfiguration());
    }


}
