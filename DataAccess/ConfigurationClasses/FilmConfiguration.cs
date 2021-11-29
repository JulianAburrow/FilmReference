using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilmReference.DataAccess
{
    public class FilmConfiguration : IEntityTypeConfiguration<Film>
    {
        public void Configure(EntityTypeBuilder<Film> builder)
        {
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
}
