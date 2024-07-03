namespace FilmReferenceDataAccess.Configuration;

public class FilmConfiguration : IEntityTypeConfiguration<FilmModel>
{
    public void Configure(EntityTypeBuilder<FilmModel> builder)
    {
        builder.ToTable("Film");
        builder.HasKey(nameof(FilmModel.FilmId));
        builder.Property(e => e.Name)
            .IsUnicode(false);
        builder.Property(e => e.Description)
            .IsUnicode(false);
        builder.HasOne(e => e.Genre)
            .WithMany(e => e.Film)
            .HasForeignKey(e => e.GenreId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(e => e.Director)
            .WithMany(e => e.Film)
            .HasForeignKey(e => e.DirectorId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(e => e.Studio)
            .WithMany(e => e.Film)
            .HasForeignKey(e => e.StudioId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
