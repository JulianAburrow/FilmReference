namespace FilmReferenceDataAccess.Configuration;

public class GenreConfiguration : IEntityTypeConfiguration<GenreModel>
{
    public void Configure(EntityTypeBuilder<GenreModel> builder)
    {
        builder.ToTable("Genre");
        builder.HasKey(nameof(GenreModel.GenreId));
        builder.HasMany(e => e.Films)
            .WithOne(e => e.Genre)
            .HasForeignKey(e => e.GenreId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
