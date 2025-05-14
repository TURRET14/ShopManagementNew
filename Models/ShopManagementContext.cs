using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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
    private ShopManagementContext()
    {
    }

    public ShopManagementContext(DbContextOptions<ShopManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerOrder> CustomerOrders { get; set; }

    public virtual DbSet<CustomerOrderItem> CustomerOrderItems { get; set; }

    public virtual DbSet<CustomerOrdersGroupByCustomer> CustomerOrdersGroupByCustomers { get; set; }

    public virtual DbSet<CustomerOrdersGroupByEmployee> CustomerOrdersGroupByEmployees { get; set; }

    public virtual DbSet<CustomerReturnItem> CustomerReturnItems { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SupplierOrder> SupplierOrders { get; set; }

    public virtual DbSet<SupplierOrderItem> SupplierOrderItems { get; set; }

    public virtual DbSet<SupplierOrdersGroupByEmployee> SupplierOrdersGroupByEmployees { get; set; }

    public virtual DbSet<SupplierOrdersGroupBySupplier> SupplierOrdersGroupBySuppliers { get; set; }

    public virtual DbSet<SupplierReturnItem> SupplierReturnItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=bd-kip.fa.ru;Database=ShopManagement;User=ShopManagementGuest;Password=JFHLKT51065!;TrustServerCertificate=True", SqlOptions =>
        {
            SqlOptions.ExecutionStrategy(Deps => new NonRetryingExecutionStrategy(Deps));
        });

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC277467C696");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
        });

        modelBuilder.Entity<CustomerOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC270707FCC6");

            entity.ToTable(tb => tb.HasTrigger("TriggerOnDeleteCustomerOrder"));

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerOrders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__CustomerO__Custo__59FA5E80");

            entity.HasOne(d => d.Employee).WithMany(p => p.CustomerOrders)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__CustomerO__Emplo__5AEE82B9");
        });

        modelBuilder.Entity<CustomerOrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC276D613D34");

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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerO__Order__5DCAEF64");

            entity.HasOne(d => d.Product).WithMany(p => p.CustomerOrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__CustomerO__Produ__5EBF139D");
        });

        modelBuilder.Entity<CustomerOrdersGroupByCustomer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("CustomerOrdersGroupByCustomer");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerName).HasMaxLength(100);
            entity.Property(e => e.OrderedCost).HasColumnType("numeric(38, 2)");
            entity.Property(e => e.ReturnedCost).HasColumnType("numeric(38, 2)");
            entity.Property(e => e.TotalCost).HasColumnType("numeric(38, 2)");
        });

        modelBuilder.Entity<CustomerOrdersGroupByEmployee>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("CustomerOrdersGroupByEmployee");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EmployeeName).HasMaxLength(100);
            entity.Property(e => e.OrderedCost).HasColumnType("numeric(38, 2)");
            entity.Property(e => e.ReturnedCost).HasColumnType("numeric(38, 2)");
            entity.Property(e => e.TotalCost).HasColumnType("numeric(38, 2)");
        });

        modelBuilder.Entity<CustomerReturnItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC27B639A1A5");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("TriggerOnDeleteCustomerReturnItem");
                    tb.HasTrigger("TriggerOnInsertCustomerReturnItem");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.OrderItemId).HasColumnName("OrderItemID");
            entity.Property(e => e.Reason)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Employee).WithMany(p => p.CustomerReturnItems)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__CustomerR__Emplo__68487DD7");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.CustomerReturnItems)
                .HasForeignKey(d => d.OrderItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerR__Order__656C112C");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC27E6757530");

            entity.HasIndex(e => e.UserLogin, "UQ__Employee__7F8E8D5E96538F8A").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Position).HasMaxLength(50);
            entity.Property(e => e.UserLogin).HasMaxLength(50);
            entity.Property(e => e.UserPassword).HasMaxLength(64);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC27A7185C7A");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("numeric(10, 2)");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC2757F59394");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AccountNumber).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
        });

        modelBuilder.Entity<SupplierOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC271FE0ECCD");

            entity.ToTable(tb => tb.HasTrigger("TriggerOnDeleteSupplierOrder"));

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

            entity.HasOne(d => d.Employee).WithMany(p => p.SupplierOrders)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__SupplierO__Emplo__47DBAE45");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierOrders)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__SupplierO__Suppl__46E78A0C");
        });

        modelBuilder.Entity<SupplierOrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC2718E62D8A");

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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupplierO__Order__4AB81AF0");

            entity.HasOne(d => d.Product).WithMany(p => p.SupplierOrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__SupplierO__Produ__4BAC3F29");
        });

        modelBuilder.Entity<SupplierOrdersGroupByEmployee>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SupplierOrdersGroupByEmployee");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EmployeeName).HasMaxLength(100);
            entity.Property(e => e.OrderedCost).HasColumnType("numeric(38, 2)");
            entity.Property(e => e.ReturnedCost).HasColumnType("numeric(38, 2)");
            entity.Property(e => e.TotalCost).HasColumnType("numeric(38, 2)");
        });

        modelBuilder.Entity<SupplierOrdersGroupBySupplier>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SupplierOrdersGroupBySupplier");

            entity.Property(e => e.OrderedCost).HasColumnType("numeric(38, 2)");
            entity.Property(e => e.ReturnedCost).HasColumnType("numeric(38, 2)");
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.SupplierName).HasMaxLength(100);
            entity.Property(e => e.TotalCost).HasColumnType("numeric(38, 2)");
        });

        modelBuilder.Entity<SupplierReturnItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC2797398842");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("TriggerOnDeleteSupplierReturnItem");
                    tb.HasTrigger("TriggerOnInsertSupplierReturnItem");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.OrderItemId).HasColumnName("OrderItemID");
            entity.Property(e => e.Reason)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Employee).WithMany(p => p.SupplierReturnItems)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__SupplierR__Emplo__5535A963");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.SupplierReturnItems)
                .HasForeignKey(d => d.OrderItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupplierR__Order__52593CB8");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
