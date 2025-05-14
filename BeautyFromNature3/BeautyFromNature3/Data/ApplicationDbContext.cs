using BeautyFromNature3.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeautyFromNature3.Data
{
    public class ApplicationDbContext : IdentityDbContext<BeautyUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
           
        }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Company> Companies { get; set; }

        public DbSet<ProductCatalog> ProductCatalogs { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<BeautyUser> BeautyUsers { get; set; }
    }
}
