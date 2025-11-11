using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AntiqueShopAvalonia.Model;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductStatus> ProductStatuses { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Return> Returns { get; set; }

    public virtual DbSet<ReturnType> ReturnTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=micialware.ru;Port=5432;Database=antique_db;Username=trieco_admin;Password=trieco");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Client_pkey");

            entity.ToTable("Client");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BankDetail)
                .HasMaxLength(16)
                .HasColumnName("bankDetail");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Fname).HasColumnName("fname");
            entity.Property(e => e.Lname).HasColumnName("lname");
            entity.Property(e => e.PassportData)
                .HasMaxLength(10)
                .HasColumnName("passportData");
            entity.Property(e => e.Patronymic).HasColumnName("patronymic");
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("roleId");

            entity.HasOne(d => d.Role).WithMany(p => p.Clients)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Client_roleId_fkey");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PaymentMethod_pkey");

            entity.ToTable("PaymentMethod");

            entity.HasIndex(e => e.Name, "PaymentMethod_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Product_pkey");

            entity.ToTable("Product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.ClientId).HasColumnName("clientId");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(18)
                .HasColumnName("price");
            entity.Property(e => e.ReceiptDate).HasColumnName("receiptDate");
            entity.Property(e => e.ShopShare)
                .HasPrecision(5, 2)
                .HasColumnName("shopShare");
            entity.Property(e => e.StatusId).HasColumnName("statusId");
            entity.Property(e => e.StoragePeriod).HasColumnName("storagePeriod");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("Product_categoryId_fkey");

            entity.HasOne(d => d.Client).WithMany(p => p.Products)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("Product_clientId_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Products)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("Product_statusId_fkey");
        });

        modelBuilder.Entity<ProductStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ProductStatus_pkey");

            entity.ToTable("ProductStatus");

            entity.HasIndex(e => e.Name, "ProductStatus_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ProductType_pkey");

            entity.ToTable("ProductType");

            entity.HasIndex(e => e.Name, "ProductType_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Return>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Return_pkey");

            entity.ToTable("Return");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.ReturnDate).HasColumnName("returnDate");
            entity.Property(e => e.ReturnTypeId).HasColumnName("returnTypeId");

            entity.HasOne(d => d.Product).WithMany(p => p.Returns)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("Return_productId_fkey");

            entity.HasOne(d => d.ReturnType).WithMany(p => p.Returns)
                .HasForeignKey(d => d.ReturnTypeId)
                .HasConstraintName("Return_returnTypeId_fkey");
        });

        modelBuilder.Entity<ReturnType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ReturnType_pkey");

            entity.ToTable("ReturnType");

            entity.HasIndex(e => e.Name, "ReturnType_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Role_pkey");

            entity.ToTable("Role");

            entity.HasIndex(e => e.Name, "Role_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Sales_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientAmount)
                .HasPrecision(18, 2)
                .HasColumnName("clientAmount");
            entity.Property(e => e.ClientId).HasColumnName("clientId");
            entity.Property(e => e.PayMethodId).HasColumnName("payMethodId");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.SaleDate).HasColumnName("saleDate");
            entity.Property(e => e.ShopAmount)
                .HasPrecision(18, 2)
                .HasColumnName("shopAmount");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(18, 2)
                .HasColumnName("totalAmount");

            entity.HasOne(d => d.Client).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("Sales_clientId_fkey");

            entity.HasOne(d => d.PayMethod).WithMany(p => p.Sales)
                .HasForeignKey(d => d.PayMethodId)
                .HasConstraintName("Sales_payMethodId_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("Sales_productId_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_pkey");

            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateBirth).HasColumnName("dateBirth");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Fname).HasColumnName("fname");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.Lname).HasColumnName("lname");
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Patronymic).HasColumnName("patronymic");
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("roleId");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_roleId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
