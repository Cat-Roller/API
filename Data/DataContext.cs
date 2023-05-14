using Microsoft.EntityFrameworkCore;
using Pokemon_Review_App_Web_API_.Models;
using System.Data;

namespace Pokemon_Review_App_Web_API_.Data
{
    public class DataContext : DbContext 
    {
       
        public DataContext(DbContextOptions<DataContext> options):base(options){}

        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }
        public DbSet<Pokemon_category> Pokemon_categories { get; set;}
        public DbSet<pokemon_owner> Pokemon_owners { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pokemon_category>()
                .HasKey(pc => new {pc.PokemonId,pc.CategoryId});
            modelBuilder.Entity<Pokemon_category>()
                .HasOne(p => p.pokemon)
                .WithMany(pc => pc.categories)
                .HasForeignKey(p=>p.PokemonId);
            modelBuilder.Entity<Pokemon_category>()
                .HasOne(p => p.category)
                .WithMany(pc => pc.pokemons)
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<pokemon_owner>()
                .HasKey(po => new { po.PokemonId, po.OwnerId });
            modelBuilder.Entity<pokemon_owner>()
                .HasOne(p => p.pokemon)
                .WithMany(po => po.owners)
                .HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<pokemon_owner>()
                .HasOne(p => p.owner)
                .WithMany(po => po.pokemons)
                .HasForeignKey(o => o.OwnerId);
        }
    }
}
