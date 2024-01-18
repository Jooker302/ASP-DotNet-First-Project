using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using VPProject.Models;

namespace VPProject;

public partial class VpprojectContext : DbContext
{
    public VpprojectContext()
    {
    }

    public VpprojectContext(DbContextOptions<VpprojectContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } 
    public DbSet<Report> Reports { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server=localhost;user=root;database=VPProject", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.28-mariadb"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
