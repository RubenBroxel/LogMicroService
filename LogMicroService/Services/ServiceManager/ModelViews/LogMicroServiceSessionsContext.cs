using System;
using System.Collections.Generic;
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
    => optionsBuilder.UseSqlServer("Server=10.100.8.2; TrustServerCertificate=True; Encrypt=false; User ID=sa;Password=Az19882009; Initial Catalog=LogMicroServiceSessions;");

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
