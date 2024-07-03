namespace FilmReferenceDataAccess.Configuration;

public class PersonConfiguration : IEntityTypeConfiguration<PersonModel>
{
    public void Configure(EntityTypeBuilder<PersonModel> builder)
    {
        builder.ToTable("Person");
        builder.HasKey(nameof(PersonModel.PersonId));
        builder.Property(e => e.FirstName)
            .IsUnicode(false);
        builder.Property(e => e.LastName)
            .IsUnicode(false);
        builder.Property(e => e.Description)
            .IsUnicode(false);
        builder.HasMany(e => e.FilmPerson)
            .WithOne(e => e.Person)
            .HasForeignKey(e => e.PersonId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
