using MVCConfigurator.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCConfigurator.DAL
{
    internal class ConfiguratorContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Part>()
                .HasMany<Part>(x => x.IncompatibleParts)
                .WithMany()
                .Map(x =>
                {
                    x.MapLeftKey("Part_Id");
                    x.MapRightKey("Part_Id1");
                    x.ToTable("PartParts");
                });

            //modelBuilder.Entity<Product>().HasMany<Part>(x => x.Parts).WithRequired().WillCascadeOnDelete(true);
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
    }
}
