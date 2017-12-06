using Db.WeHaveSecrets.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Db.WeHaveSecrets
{
    public class WeHaveSecretsContext: DbContext
    {
        public WeHaveSecretsContext(DbContextOptions<WeHaveSecretsContext> options): base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Secret> Secrets { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(t => new { t.UserId, t.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(u => u.User)
                .WithMany(ur => ur.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
        }

        public void EnsureSeedData()
        {
            if (!Roles.AnyAsync().Result)
            {
                Roles.AddRange(
                    new Role { Id = "User", Description = "Standard Role" },
                    new Role { Id = "Admin", Description = "Administrator Role" }
                );

                SaveChanges();
            }
        }
    }
}
