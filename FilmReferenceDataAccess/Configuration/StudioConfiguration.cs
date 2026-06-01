namespace FilmReferenceDataAccess.Configuration;

public class StudioConfiguration : IEntityTypeConfiguration<StudioModel>
{
    public void Configure(EntityTypeBuilder<StudioModel> builder)
    {
        builder.ToTable("Studio");
        builder.HasKey(f => f.StudioId);
        builder.HasMany(e => e.Films)
            .WithOne(e => e.Studio)
            .HasForeignKey(e => e.StudioId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
