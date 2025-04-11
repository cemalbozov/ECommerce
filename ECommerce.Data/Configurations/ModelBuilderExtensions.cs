using ECommerce.Entity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Data.Configurations
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(
                new Category() { Id = 1, Name = "Iphone", Url = "iphone", ParentCategoryId = 1 },
                new Category() { Id = 2, Name = "Android Telefon", Url = "android-telefon", ParentCategoryId = 1 },
                new Category() { Id = 3, Name = "Aksesuar", Url = "aksesuar", ParentCategoryId = 1 },
                new Category() { Id = 4, Name = "Laptop", Url = "laptop", ParentCategoryId = 2 },
                new Category() { Id = 5, Name = "Masaüstü", Url = "masaustu", ParentCategoryId = 2 },
                new Category() { Id = 6, Name = "Tablet", Url = "tablet", ParentCategoryId = 2 },
                new Category() { Id = 7, Name = "Televizyon", Url = "televizyon", ParentCategoryId = 3 },
                new Category() { Id = 8, Name = "Hoparlör", Url = "hoparlor", ParentCategoryId = 3 }
                );

            builder.Entity<ParentCategory>().HasData(
                new ParentCategory() { Id = 1, Name = "Telefon & Aksesuar", },
                new ParentCategory() { Id = 2, Name = "Bilgisayar & Tablet", },
                new ParentCategory() { Id = 3, Name = "Televizyon,Ses ve Görüntü", }
                );

            builder.Entity<ProductCategory>().HasData(
                new ProductCategory() { CategoryId = 1, ProductId = 1 },
                new ProductCategory() { CategoryId = 1, ProductId = 2 },
                new ProductCategory() { CategoryId = 4, ProductId = 3 },
                new ProductCategory() { CategoryId = 3, ProductId = 4 }
                );

            builder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "Iphone 14 Pro", Url = "iphone-14-pro", Description = "İyi Telefon", Price = 50000, ImageUrl = "4.jpeg", IsApproved = true },
                new Product() { Id = 2, Name = "Iphone 13 Pro", Url = "iphone-13-pro", Description = "güzel Telefon", Price = 30000, ImageUrl = "7.jpeg", IsApproved = true },
                new Product() { Id = 3, Name = "Msi Dragon", Url = "msi-dragon", Description = "İyi bilgisayar", Price = 32000, ImageUrl = "15.jpeg", IsApproved = true },
                new Product() { Id = 4, Name = "Apple Watch", Url = "apple-watch", Description = "İyi saat", Price = 10000, ImageUrl = "2.jpeg", IsApproved = true }
                );

            builder.Entity<IdentityRole>().HasData(
               new IdentityRole
               {
                   Name = "Customer",
                   NormalizedName = "CUSTOMER"
               },
               new IdentityRole
               {
                   Name = "Admin",
                   NormalizedName = "ADMIN"
               }
           );
        }
    }
}
