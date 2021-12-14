using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Database : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<AuthorRate> AuthorsRate { get; set; }
        public DbSet<BookRate> BooksRate { get; set; }

        public Database()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source=d:\\studia_II_stopien\\PU\\LAB\\LAB_2\\PU_projekt2\\Model\\database.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Rate>()
                .HasDiscriminator(x => x.Type)
                .HasValue<AuthorRate>(RateType.AuthorRate)
                .HasValue<BookRate>(RateType.BookRate);
        }
    }
}
