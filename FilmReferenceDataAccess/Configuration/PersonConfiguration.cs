namespace FilmReferenceDataAccess.Configuration;

public class PersonConfiguration : IEntityTypeConfiguration<PersonModel>
{
    public void Configure(EntityTypeBuilder<PersonModel> builder)
    {
        builder.ToTable("Person");
        builder.HasKey(f => f.PersonId);
        builder.HasMany(e => e.Films)
            .WithOne(e => e.Director)
            .HasForeignKey(e => e.DirectorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasMany(e => e.FilmPerson)
            .WithOne(e => e.Person)
            .HasForeignKey(e => e.PersonId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Property(e => e.DateOfBirth)
            .HasColumnType("DATETIME2");
        builder.Property(e => e.DateOfDeath)
            .HasColumnType("DATETIME2");
    }
}
