namespace FilmReferenceDataAccess.Configuration;

public class FavouriteConfiguration : IEntityTypeConfiguration<FavouriteModel>
{
    public void Configure(EntityTypeBuilder<FavouriteModel> builder)
    {
        builder.ToTable("Favourite");
        builder.HasKey(f => f.FavouriteId);
    }
}
