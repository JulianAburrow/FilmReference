namespace FilmReferenceDataAccess.Configuration;

public class StudioConfiguration : IEntityTypeConfiguration<StudioModel>
{
    public void Configure(EntityTypeBuilder<StudioModel> builder)
    {
        builder.ToTable("Studio");
        builder.HasKey(nameof(StudioModel.StudioId));
        builder.Property(e => e.Name)
            .IsUnicode(false);
        builder.Property(e => e.Description)
            .IsUnicode(false);
        builder.HasMany(e => e.Films)
            .WithOne(e => e.Studio)
            .HasForeignKey(e => e.StudioId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
