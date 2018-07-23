using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Motohusaria.DomainClasses;

namespace Motohusaria.DataLayer
{
    public class MotohusariaDbContext : DbContext
    {
        public MotohusariaDbContext(DbContextOptions<MotohusariaDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        public DbSet<Role> Role { get; set; }

        public DbSet<UserRole> UserRole { get; set; }

        public DbSet<Article> Article { get; set; }

        public DbSet<Calendario> Calendario { get; set; }
    }

    public class DesignTimeDbContextFactory : Microsoft.EntityFrameworkCore.Design.IDesignTimeDbContextFactory<MotohusariaDbContext>
    {
    public MotohusariaDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<MotohusariaDbContext>();
            var connectionString = configuration.GetConnectionString("Default");
            builder.UseSqlServer(connectionString);
            return new MotohusariaDbContext(builder.Options);
        }
    }
}
