using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShopManagement.Models;

public partial class ShopManagementContext : DbContext
{
    static private ShopManagementContext Context;
    static public ShopManagementContext GetContext()
    {
        if (Context == null)
        {
            Context = new ShopManagementContext();
        }
        return Context;
    }
    public ShopManagementContext()
    {
    }

    public ShopManagementContext(DbContextOptions<ShopManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerOrder> CustomerOrders { get; set; }

    public virtual DbSet<CustomerOrderItem> CustomerOrderItems { get; set; }

    public virtual DbSet<CustomerReturnItem> CustomerReturnItems { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SupplierOrder> SupplierOrders { get; set; }

    public virtual DbSet<SupplierOrderItem> SupplierOrderItems { get; set; }

    public virtual DbSet<SupplierReturnItem> SupplierReturnItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=bd-kip.fa.ru;User ID=sa;Password=1qaz!QAZ;Database=ShopManagement;Trusted_Connection=False;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC27F3E9D0FE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
        });

        modelBuilder.Entity<CustomerOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC2719C50DB7");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerOrders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__CustomerO__Custo__59063A47");

            entity.HasOne(d => d.Employee).WithMany(p => p.CustomerOrders)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__CustomerO__Emplo__59FA5E80");
        });

        modelBuilder.Entity<CustomerOrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC27EB930970");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("TriggerOnDeleteCustomerOrderItem");
                    tb.HasTrigger("TriggerOnInsertCustomerOrderItem");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasDefaultValue(1);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Price).HasColumnType("numeric(10, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Order).WithMany(p => p.CustomerOrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__CustomerO__Order__5CD6CB2B");

            entity.HasOne(d => d.Product).WithMany(p => p.CustomerOrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__CustomerO__Produ__5DCAEF64");
        });

        modelBuilder.Entity<CustomerReturnItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC2729B1E6DB");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("TriggerOnDeleteCustomerReturnItem");
                    tb.HasTrigger("TriggerOnInsertCustomerReturnItem");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasDefaultValue(1);
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.OrderItemId).HasColumnName("OrderItemID");
            entity.Property(e => e.Reason)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Employee).WithMany(p => p.CustomerReturnItems)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__CustomerR__Emplo__6754599E");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.CustomerReturnItems)
                .HasForeignKey(d => d.OrderItemId)
                .HasConstraintName("FK__CustomerR__Order__6477ECF3");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC27C065025F");

            entity.HasIndex(e => e.UserLogin, "UQ__Employee__7F8E8D5E5D551750").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.Position).HasMaxLength(50);
            entity.Property(e => e.UserLogin).HasMaxLength(50);
            entity.Property(e => e.UserPassword).HasMaxLength(64);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC27D33D3946");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("numeric(10, 2)");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC272D73ECE8");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AccountNumber).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
        });

        modelBuilder.Entity<SupplierOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC27F4DF4FDB");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

            entity.HasOne(d => d.Employee).WithMany(p => p.SupplierOrders)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__SupplierO__Emplo__46E78A0C");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierOrders)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__SupplierO__Suppl__45F365D3");
        });

        modelBuilder.Entity<SupplierOrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC27FB32C95C");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("TriggerOnDeleteSupplierOrderItem");
                    tb.HasTrigger("TriggerOnInsertSupplierOrderItem");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasDefaultValue(1);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Price).HasColumnType("numeric(10, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Order).WithMany(p => p.SupplierOrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__SupplierO__Order__49C3F6B7");

            entity.HasOne(d => d.Product).WithMany(p => p.SupplierOrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__SupplierO__Produ__4AB81AF0");
        });

        modelBuilder.Entity<SupplierReturnItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC271F269AE3");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("TriggerOnDeleteSupplierReturnItem");
                    tb.HasTrigger("TriggerOnInsertSupplierReturnItem");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasDefaultValue(1);
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.OrderItemId).HasColumnName("OrderItemID");
            entity.Property(e => e.Reason)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Employee).WithMany(p => p.SupplierReturnItems)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__SupplierR__Emplo__5441852A");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.SupplierReturnItems)
                .HasForeignKey(d => d.OrderItemId)
                .HasConstraintName("FK__SupplierR__Order__5165187F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
