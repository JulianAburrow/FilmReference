namespace FilmReferenceDataAccess.Configuration;

public class NationalityConfiguration : IEntityTypeConfiguration<NationalityModel>
{
    public void Configure(EntityTypeBuilder<NationalityModel> builder)
    {
        builder.ToTable("Nationality");
        builder.HasKey(n => n.NationalityId);
        builder.HasMany(n => n.People)
               .WithOne(p => p.Nationality)
               .HasForeignKey(p => p.NationalityId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
