using CleanMovie.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanMovie.Infrastructure
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) 
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1:n (Member, Rentals)
            modelBuilder.Entity<Member>()
                .HasOne(s => s.Rental)
                .WithMany(r => r.Members)
                .HasForeignKey(s => s.RentalId);

            // n:m (Rental, Movie)
            modelBuilder.Entity<MovieRental>()
                .HasKey(g => new { g.RentalId, g.MovieId });

            // decimal 설정
            modelBuilder.Entity<Rental>()
                .Property(p => p.TotalCost)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Movie>()
                .Property(p => p.RentalCost)
                .HasColumnType("decimal(18,2)");

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MovieRental> MovieRentals { get; set; }
    }
}
