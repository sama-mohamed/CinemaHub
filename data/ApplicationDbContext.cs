using CinemaWebSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using CinemaWebSite.Models.ViewModels;

namespace CinemaWebSite.data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ActorMovie> ActorMovies { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ActorMovie>()
            .HasKey(e => new { e.ActorId, e.MovieId });
        }
        public DbSet<CinemaWebSite.Models.ViewModels.RegisterVM> RegisterVM { get; set; } = default!;
        public DbSet<CinemaWebSite.Models.ViewModels.LoginVM> LoginVM { get; set; } = default!;


    }
}
