using Motohusaria.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Motohusaria.DataLayer
{
    public class LoggingDbContext : DbContext
    {
        public const string Schema = "log";
        public const string MigrationsHistoryTable = "__EFMigrationsHistory";
        public LoggingDbContext(DbContextOptions<LoggingDbContext> options) : base(options)
        {
        }

        public DbSet<Log> Log { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(Schema);

            modelBuilder.Entity<Log>().HasKey(k => k.Id).ForSqlServerIsClustered(false);
            modelBuilder.Entity<Log>().HasIndex(nameof(DomainClasses.Log.CreateDate), nameof(DomainClasses.Log.Id)).ForSqlServerIsClustered();
        }
    }

    public class DesignTimeLoggingDbContextFactory : Microsoft.EntityFrameworkCore.Design.IDesignTimeDbContextFactory<LoggingDbContext>
    {
        public LoggingDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<LoggingDbContext>();
            var connectionString = configuration.GetConnectionString("Logging");
            builder.UseSqlServer(connectionString,
                x => x.MigrationsHistoryTable(LoggingDbContext.MigrationsHistoryTable, LoggingDbContext.Schema));
            return new LoggingDbContext(builder.Options);
        }
    }
}
