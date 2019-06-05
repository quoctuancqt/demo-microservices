using Demo.Infrastructure.UnitOfWork;
using Demo.ProductService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;

namespace Demo.ProductService
{
    public class ProductContext : BaseAppContext
    {
        public ProductContext(IHttpContextAccessor httpContextAccessor, DbContextOptions options) : base(httpContextAccessor, options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
              new Category
              {
                  Id = Guid.NewGuid(),
                  Name = "Electronics",
                  Description = "Electronic Items",
              },
              new Category
              {
                  Id = Guid.NewGuid(),
                  Name = "Clothes",
                  Description = "Dresses",
              },
              new Category
              {
                  Id = Guid.NewGuid(),
                  Name = "Grocery",
                  Description = "Grocery Items",
              }
            );

            modelBuilder.Entity<Product>().Property(x => x.Price).HasColumnType("money");
        }
    }
}
