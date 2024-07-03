namespace FilmReferenceDataAccess.Configuration;

public class FilmPersonConfiguration : IEntityTypeConfiguration<FilmPersonModel>
{
    public void Configure(EntityTypeBuilder<FilmPersonModel> builder)
    {
        builder.ToTable("FilmPerson");
        builder.HasKey(nameof(FilmPersonModel.FilmPersonId));
        builder.HasOne(e => e.Film)
            .WithMany(e => e.FilmPerson)
            .HasForeignKey(e => e.FilmId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(e => e.Person)
            .WithMany(e => e.FilmPerson)
            .HasForeignKey(e => e.PersonId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
