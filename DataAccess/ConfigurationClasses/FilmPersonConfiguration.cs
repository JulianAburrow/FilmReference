using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilmReference.DataAccess
{
    public class FilmPersonConfiguration : IEntityTypeConfiguration<FilmPerson>
    {
        public void Configure(EntityTypeBuilder<FilmPerson> builder)
        {
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
}
