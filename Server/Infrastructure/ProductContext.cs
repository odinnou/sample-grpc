using Bogus;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Infrastructure
{
    public class ProductContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public ProductContext(DbContextOptions<ProductContext> options)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
              : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Models.Product>().HasKey(product => new { product.Reference, product.Declination });
        }

        public DbSet<Models.Product> Products { get; set; }

        public async Task SeedDatas()
        {
            Products.RemoveRange(Products);
            await SaveChangesAsync();

            List<string> declinations = new List<string> { "FRA", "BEL", "USA", "TUR", "AUS", "ITA", "ESP", "POR" };
            List<bool> isBios = new List<bool> { false, false, true };

            int order = 0;
            Faker<Models.Product> fakeGenerator = new Faker<Models.Product>()
                                                  .RuleFor(p => p.Declination, gen => gen.PickRandom(declinations))
                                                  .RuleFor(p => p.IsBio, gen => gen.PickRandom(isBios))
                                                  .RuleFor(p => p.Reference, gen => gen.Random.String2(10, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"))
                                                  .RuleFor(p => p.Order, gen => order++);

            List<Models.Product> fakes = fakeGenerator.Generate(50000);
            Products.AddRange(fakes);
            await SaveChangesAsync();
        }
    }
}
