using System;
using System.Collections.Generic;
using MP4practic.Models;
using Microsoft.EntityFrameworkCore;

namespace MP4practic.Context;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    private static PostgresContext _context;

    public static PostgresContext GetContext()
    {
        if (_context == null)
        {
            _context = new PostgresContext();
        }

        return _context;
    }
    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoryproduct> Categoryproducts { get; set; }

    public virtual DbSet<Deliveryproduct> Deliveryproducts { get; set; }

    public virtual DbSet<Manufacturerproduct> Manufacturerproducts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderproduct> Orderproducts { get; set; }

    public virtual DbSet<Pickuppoint> Pickuppoints { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Statusorder> Statusorders { get; set; }

    public virtual DbSet<Unitofmeasurement> Unitofmeasurements { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=192.168.2.159;Database=postgres;Username=user002;Password=28310");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoryproduct>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("categoryproduct_pkey");

            entity.ToTable("categoryproduct");

            entity.Property(e => e.Categoryid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("categoryid");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(100)
                .HasColumnName("categoryname");
        });

        modelBuilder.Entity<Deliveryproduct>(entity =>
        {
            entity.HasKey(e => e.Deliveryid).HasName("deliveryproduct_pkey");

            entity.ToTable("deliveryproduct");

            entity.Property(e => e.Deliveryid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("deliveryid");
            entity.Property(e => e.Deliveryname)
                .HasMaxLength(100)
                .HasColumnName("deliveryname");
        });

        modelBuilder.Entity<Manufacturerproduct>(entity =>
        {
            entity.HasKey(e => e.Manufacturerid).HasName("manufacturerproduct_pkey");

            entity.ToTable("manufacturerproduct");

            entity.Property(e => e.Manufacturerid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("manufacturerid");
            entity.Property(e => e.Manufacturername)
                .HasMaxLength(100)
                .HasColumnName("manufacturername");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("Order_pkey");

            entity.ToTable("Order");

            entity.Property(e => e.Orderid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("orderid");
            entity.Property(e => e.Ordercodetoget).HasColumnName("ordercodetoget");
            entity.Property(e => e.Orderdate)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("orderdate");
            entity.Property(e => e.Orderdeliverydate)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("orderdeliverydate");
            entity.Property(e => e.Orderpickuppoint).HasColumnName("orderpickuppoint");
            entity.Property(e => e.Orderstatus).HasColumnName("orderstatus");
            entity.Property(e => e.Orderuserid).HasColumnName("orderuserid");

            entity.HasOne(d => d.OrderpickuppointNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Orderpickuppoint)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Order_orderpickuppoint_fkey");

            entity.HasOne(d => d.Orderuser).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Orderuserid)
                .HasConstraintName("Order_orderuserid_fkey");
        });

        modelBuilder.Entity<Orderproduct>(entity =>
        {
            entity.HasKey(e => new { e.Orderid, e.Productarticlenumber }).HasName("orderproduct_pkey");

            entity.ToTable("orderproduct");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Productarticlenumber)
                .HasMaxLength(100)
                .HasColumnName("productarticlenumber");
            entity.Property(e => e.Productcount).HasColumnName("productcount");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderproducts)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderproduct_orderid_fkey");

            entity.HasOne(d => d.ProductarticlenumberNavigation).WithMany(p => p.Orderproducts)
                .HasForeignKey(d => d.Productarticlenumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderproduct_productarticlenumber_fkey");
        });

        modelBuilder.Entity<Pickuppoint>(entity =>
        {
            entity.HasKey(e => e.Pickuppointid).HasName("pickuppoint_pkey");

            entity.ToTable("pickuppoint");

            entity.Property(e => e.Pickuppointid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("pickuppointid");
            entity.Property(e => e.Pickuppointeraddress)
                .HasMaxLength(100)
                .HasColumnName("pickuppointeraddress");
            entity.Property(e => e.Pickuppointercode).HasColumnName("pickuppointercode");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Productarticlenumber).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.Productarticlenumber)
                .HasMaxLength(100)
                .HasColumnName("productarticlenumber");
            entity.Property(e => e.Productcategory).HasColumnName("productcategory");
            entity.Property(e => e.Productcost)
                .HasPrecision(19, 4)
                .HasColumnName("productcost");
            entity.Property(e => e.Productdelivery).HasColumnName("productdelivery");
            entity.Property(e => e.Productdescription).HasColumnName("productdescription");
            entity.Property(e => e.Productdiscountamount).HasColumnName("productdiscountamount");
            entity.Property(e => e.Productmanufacturer).HasColumnName("productmanufacturer");
            entity.Property(e => e.Productname).HasColumnName("productname");
            entity.Property(e => e.Productphoto).HasColumnName("productphoto");
            entity.Property(e => e.Productquantityinstock).HasColumnName("productquantityinstock");
            entity.Property(e => e.Productsall).HasColumnName("productsall");
            entity.Property(e => e.Productunitofmeasurement).HasColumnName("productunitofmeasurement");

            entity.HasOne(d => d.ProductcategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Productcategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_productcategory_fkey");

            entity.HasOne(d => d.ProductdeliveryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Productdelivery)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_fk");

            entity.HasOne(d => d.ProductmanufacturerNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Productmanufacturer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_productmanufacturer_fkey");

            entity.HasOne(d => d.ProductunitofmeasurementNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Productunitofmeasurement)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_productunitofmeasurement_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.Roleid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("roleid");
            entity.Property(e => e.Rolename)
                .HasMaxLength(100)
                .HasColumnName("rolename");
        });

        modelBuilder.Entity<Statusorder>(entity =>
        {
            entity.HasKey(e => e.Statusid).HasName("statusorder_pkey");

            entity.ToTable("statusorder");

            entity.Property(e => e.Statusid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("statusid");
            entity.Property(e => e.Statusname)
                .HasMaxLength(100)
                .HasColumnName("statusname");
        });

        modelBuilder.Entity<Unitofmeasurement>(entity =>
        {
            entity.HasKey(e => e.Unitofmeasurementid).HasName("unitofmeasurement_pkey");

            entity.ToTable("unitofmeasurement");

            entity.Property(e => e.Unitofmeasurementid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("unitofmeasurementid");
            entity.Property(e => e.Unitofmeasurement1)
                .HasMaxLength(100)
                .HasColumnName("unitofmeasurement");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("User_pkey");

            entity.ToTable("User");

            entity.Property(e => e.Userid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("userid");
            entity.Property(e => e.Userlogin).HasColumnName("userlogin");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
            entity.Property(e => e.Userpassword).HasColumnName("userpassword");
            entity.Property(e => e.Userpatronymic)
                .HasMaxLength(100)
                .HasColumnName("userpatronymic");
            entity.Property(e => e.Userrole).HasColumnName("userrole");
            entity.Property(e => e.Usersurname)
                .HasMaxLength(100)
                .HasColumnName("usersurname");

            entity.HasOne(d => d.UserroleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Userrole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_userrole_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
