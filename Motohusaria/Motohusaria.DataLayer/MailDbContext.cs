using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Motohusaria.DomainClasses;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Motohusaria.DataLayer
{
    public class MailDbContext : DbContext
{
    public const string Schema = "sys";
    public const string MigrationsHistoryTable = "__EFMigrationsHistory";
    public MailDbContext(DbContextOptions<MailDbContext> options) : base(options)
    {
    }

    public DbSet<Mail> Mails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.Entity<Mail>().HasKey(k => k.Id).ForSqlServerIsClustered(false);
        modelBuilder.Entity<Mail>().HasIndex(nameof(Mail.CreatedOn), nameof(Mail.Id)).ForSqlServerIsClustered();
    }


    public class DesignTimeMailDbContextFactory : Microsoft.EntityFrameworkCore.Design.IDesignTimeDbContextFactory<MailDbContext>
    {
        public MailDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            var builder = new DbContextOptionsBuilder<MailDbContext>();
            var connectionString = configuration.GetConnectionString("Emailing");
            builder.UseSqlServer(connectionString,
                x => x.MigrationsHistoryTable(MailDbContext.MigrationsHistoryTable, MailDbContext.Schema));
            return new MailDbContext(builder.Options);
        }
    }
}
}
