using Microsoft.EntityFrameworkCore;

namespace FilmReference.DataAccess
{
    public class FilmReferenceContext : DbContext
    {
        public FilmReferenceContext(DbContextOptions<FilmReferenceContext> options)
            : base(options)
        {
        }

        #region DbSets

        public DbSet<Film> Film { get; set;}
        public DbSet<FilmPerson> FilmPerson { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Studio> Studio { get; set; }

        #endregion

        #region OnModelCreating

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new FilmConfiguration());
            builder.ApplyConfiguration(new FilmPersonConfiguration());
            builder.ApplyConfiguration(new GenreConfiguration());
            builder.ApplyConfiguration(new PersonConfiguration());
            builder.ApplyConfiguration(new StudioConfiguration());
        }

        #endregion
    }
}
