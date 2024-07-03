namespace FilmReferenceDataAccess.Configuration;

public class GenreConfiguration : IEntityTypeConfiguration<GenreModel>
{
    public void Configure(EntityTypeBuilder<GenreModel> builder)
    {
        builder.ToTable("Genre");
        builder.HasKey(nameof(GenreModel.GenreId));
        builder.Property(e => e.Name)
            .IsUnicode(false);
        builder.Property(e => e.Description)
            .IsUnicode(false);
        builder.HasMany(e => e.Film)
            .WithOne(e => e.Genre)
            .HasForeignKey(e => e.GenreId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
