using System;
using Microsoft.EntityFrameworkCore;
using Books.Database.Entities;

namespace Books.Database
{
    public class BooksContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=BookDemo;User ID=SA;Password=P@ssw0rd12;MultipleActiveResultSets=true");
        }
    }
}

