using System;
using Microsoft.EntityFrameworkCore;

namespace PhoneBook.Model
{
    public class PhoneBookContext : DbContext
    {
        public PhoneBookContext(DbContextOptions<PhoneBookContext> options) : base(options)
        {
        }

        DbSet<PhoneBookEntry> PhoneBook { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PhoneBookEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(250);
                /* Internation Telecommunication Union has phone numer limit of 15 digits
                 * but need to account for extra characters like ( ), -, +, and space */
                entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            });
        }
    }
}

