using ECommerce.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Data.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);

            builder.Property(p => p.DateAdded).HasDefaultValueSql("getdate()");

            /*builder.HasData(
                new Product() { Id = 1, Name = "Iphone 14 Pro", Url = "iphone-14-pro", Description = "İyi Telefon", Price = 50000, ImageUrl = "4.jpeg", IsApproved = true },
                new Product() { Id = 2, Name = "Iphone 13 Pro", Url = "iphone-13-pro", Description = "güzel Telefon", Price = 30000, ImageUrl = "7.jpeg", IsApproved = true },
                new Product() { Id = 3, Name = "Msi Dragon", Url = "msi-dragon", Description = "İyi bilgisayar", Price = 32000, ImageUrl = "15.jpeg", IsApproved = true },
                new Product() { Id = 4, Name = "Apple Watch", Url = "apple-watch", Description = "İyi saat", Price = 10000, ImageUrl = "2.jpeg", IsApproved = true }

                );*/
        }
    }
}
