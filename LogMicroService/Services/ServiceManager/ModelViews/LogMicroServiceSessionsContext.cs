using System;
using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;

namespace LogMicroService.Services.ServiceManager.ModelViews;

public partial class LogMicroServiceSessionsContext : DbContext
{
    public LogMicroServiceSessionsContext()
    {
    }

    public LogMicroServiceSessionsContext(DbContextOptions<LogMicroServiceSessionsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LogServiceSession> LogServiceSessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile("appsettings.json");
        var config = builder.Build();
        var connectionString = config.GetConnectionString("LogMicroService");

        if (!optionsBuilder.IsConfigured)
        {
            //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
    //=> optionsBuilder.UseSqlServer("Server=10.100.8.2; TrustServerCertificate=True; Encrypt=false; User ID=sa;Password=Az19882009; Initial Catalog=LogMicroServiceSessions;");
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogServiceSession>(entity =>
        {
            entity.HasKey(e => e.IdSession);

            entity.Property(e => e.AppBuild).HasMaxLength(50);
            entity.Property(e => e.AppName).HasMaxLength(50);
            entity.Property(e => e.AppPackage).HasMaxLength(50);
            entity.Property(e => e.AppVersion).HasMaxLength(8);
            entity.Property(e => e.CountrySession).HasMaxLength(20);
            entity.Property(e => e.DateSession).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IpAddress).HasMaxLength(17);
            entity.Property(e => e.LocationSession).HasColumnType("ntext");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
