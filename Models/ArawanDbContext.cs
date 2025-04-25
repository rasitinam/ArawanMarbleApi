using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ArawanMarbleApi.Models;

public partial class ArawanDbContext : DbContext
{
    public ArawanDbContext()
    {
    }

    public ArawanDbContext(DbContextOptions<ArawanDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=./ArawanDb.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("customers");

            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Customeremail).HasColumnName("customeremail");
            entity.Property(e => e.Customermessage).HasColumnName("customermessage");
            entity.Property(e => e.Customername).HasColumnName("customername");
            entity.Property(e => e.Customersubject).HasColumnName("customersubject");
            entity.Property(e => e.Phone).HasColumnName("phone");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");

            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Productimg).HasColumnName("productimg");
            entity.Property(e => e.Productname).HasColumnName("productname");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("projects");

            entity.Property(e => e.Projectid).HasColumnName("projectid");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Projectimg).HasColumnName("projectimg");
            entity.Property(e => e.Projectname).HasColumnName("projectname");
            entity.Property(e => e.Projectplace).HasColumnName("projectplace");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "IX_users_username").IsUnique();

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Username).HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
