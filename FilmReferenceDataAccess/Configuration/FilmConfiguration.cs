namespace FilmReferenceDataAccess.Configuration;

public class FilmConfiguration : IEntityTypeConfiguration<FilmModel>
{
    public void Configure(EntityTypeBuilder<FilmModel> builder)
    {
        builder.ToTable("Film");
        builder.HasKey(f => f.FilmId);
        builder.HasOne(e => e.Genre)
            .WithMany(e => e.Films)
            .HasForeignKey(e => e.GenreId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(e => e.Studio)
            .WithMany(e => e.Films)
            .HasForeignKey(e => e.StudioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(e => e.Director)
            .WithMany(e => e.Films)
            .HasForeignKey(e => e.DirectorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasMany(e => e.FilmPerson)
            .WithOne(e => e.Film)
            .HasForeignKey(e => e.FilmId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

    }
}
