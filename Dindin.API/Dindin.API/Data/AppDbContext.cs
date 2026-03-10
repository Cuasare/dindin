﻿using Dindin.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Dindin.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Gastos> Gastos { get; set; }
    public DbSet<Depositos> Depositos { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Group> Grupo { get; set; }
    public DbSet<UserGroup> UserGrupo { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasMany(u => u.Gastos)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Depositos)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.HasKey(ug => new { ug.UserId, ug.GrupoId });

            entity.HasOne(ug => ug.User)
                .WithMany(u => u.UserGroup)
                .HasForeignKey(ug => ug.UserId)
                .IsRequired();

            entity.HasOne(ug => ug.Grupo)
                .WithMany(g => g.UserGrupo)
                .HasForeignKey(ug => ug.GrupoId)
                .IsRequired();
        });
    }
}