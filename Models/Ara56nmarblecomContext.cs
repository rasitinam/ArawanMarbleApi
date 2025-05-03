using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ArawanMarbleApi.Models;

public partial class Ara56nmarblecomContext : DbContext
{
    public Ara56nmarblecomContext()
    {
    }

    public Ara56nmarblecomContext(DbContextOptions<Ara56nmarblecomContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<SubProduct> SubProducts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=31.186.11.163,1433;Database=ara56nmarblecom_;User Id=qweqwe;Password=43_W5uv0u;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("qweqwe");

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Customerid).HasName("PK__customer__B61ED7F57451DEB7");

            entity.ToTable("customers");

            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Customeremail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("customeremail");
            entity.Property(e => e.Customermessage)
                .HasColumnType("text")
                .HasColumnName("customermessage");
            entity.Property(e => e.Customername)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("customername");
            entity.Property(e => e.Customersubject)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("customersubject");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Productid).HasName("PK__Product__B40F3AA5492CBE86");

            entity.ToTable("Product");

            entity.Property(e => e.Productimg).HasMaxLength(200);
            entity.Property(e => e.Productname).HasMaxLength(30);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Projectid).HasName("PK__projects__11EC39DD7720BDC2");

            entity.ToTable("projects");

            entity.Property(e => e.Projectid).HasColumnName("projectid");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Projectimg)
                .HasMaxLength(255)
                .HasColumnName("projectimg");
            entity.Property(e => e.Projectname)
                .HasMaxLength(30)
                .HasColumnName("projectname");
            entity.Property(e => e.Projectplace)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("projectplace");
        });

        modelBuilder.Entity<SubProduct>(entity =>
        {
            entity.HasKey(e => e.SubProductid).HasName("PK__SubProdu__65CA1C9D6DE7087D");

            entity.ToTable("SubProduct");

            entity.Property(e => e.Productimg).HasMaxLength(200);
            entity.Property(e => e.Productname).HasMaxLength(30);

            entity.HasOne(d => d.Product).WithMany(p => p.SubProducts)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("FK__SubProduc__Produ__4E88ABD4");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("PK__users__CBA1B25754546766");

            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "UQ__users__F3DBC5724A16B652").IsUnique();

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
