using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MedicineWebsite.Domain.Entities;
using MedicineWebsite.Domain.Enums;

namespace MedicineWebsite.Persistence.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Medicine> Medicines { get; set; }
    public DbSet<Pharmacy> Pharmacies { get; set; }
    public DbSet<PharmacyMedicine> PharmacyMedicines { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }
    public DbSet<UserFavoritePharmacy> UserFavoritePharmacies { get; set; }
    public DbSet<MedicineAlternative> MedicineAlternatives { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // User entity configuration
        builder.Entity<User>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.State).HasMaxLength(100);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        // Medicine entity configuration
        builder.Entity<Medicine>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.GenericName).HasMaxLength(200);
            entity.Property(e => e.Manufacturer).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Strength).HasMaxLength(50);
            entity.Property(e => e.DosageForm).HasMaxLength(50);
            entity.Property(e => e.ActiveIngredients).HasMaxLength(500);
            entity.Property(e => e.DrugClass).HasMaxLength(100);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.RxNormId).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.GenericName);
            entity.HasIndex(e => e.RxNormId);
        });

        // Pharmacy entity configuration
        builder.Entity<Pharmacy>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.LicenseNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Address).HasMaxLength(500).IsRequired();
            entity.Property(e => e.City).HasMaxLength(100).IsRequired();
            entity.Property(e => e.State).HasMaxLength(100).IsRequired();
            entity.Property(e => e.PostalCode).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Country).HasMaxLength(100).IsRequired();
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Website).HasMaxLength(200);
            entity.Property(e => e.WorkingDays).HasMaxLength(200);
            entity.Property(e => e.DeliveryFee).HasColumnType("decimal(10,2)");
            entity.Property(e => e.MinimumOrderAmount).HasColumnType("decimal(10,2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.HasIndex(e => e.LicenseNumber).IsUnique();
            entity.HasIndex(e => new { e.Latitude, e.Longitude });
        });

        // PharmacyMedicine entity configuration
        builder.Entity<PharmacyMedicine>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Price).HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.DiscountPrice).HasColumnType("decimal(10,2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            
            entity.HasOne(e => e.Pharmacy)
                .WithMany(p => p.PharmacyMedicines)
                .HasForeignKey(e => e.PharmacyId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Medicine)
                .WithMany(m => m.PharmacyMedicines)
                .HasForeignKey(e => e.MedicineId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasIndex(e => new { e.PharmacyId, e.MedicineId }).IsUnique();
        });

        // Order entity configuration
        builder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SubTotal).HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.DeliveryFee).HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.Status).HasConversion<int>();
            entity.Property(e => e.PaymentStatus).HasConversion<int>();
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.PaymentTransactionId).HasMaxLength(100);
            entity.Property(e => e.DeliveryAddress).HasMaxLength(500);
            entity.Property(e => e.DeliveryCity).HasMaxLength(100);
            entity.Property(e => e.DeliveryState).HasMaxLength(100);
            entity.Property(e => e.DeliveryPostalCode).HasMaxLength(20);
            entity.Property(e => e.DeliveryCountry).HasMaxLength(100);
            entity.Property(e => e.CustomerNotes).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Pharmacy)
                .WithMany(p => p.Orders)
                .HasForeignKey(e => e.PharmacyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // OrderItem entity configuration
        builder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            
            entity.HasOne(e => e.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Medicine)
                .WithMany(m => m.OrderItems)
                .HasForeignKey(e => e.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // OrderStatusHistory entity configuration
        builder.Entity<OrderStatusHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).HasConversion<int>();
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            
            entity.HasOne(e => e.Order)
                .WithMany(o => o.StatusHistory)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // UserFavoritePharmacy entity configuration
        builder.Entity<UserFavoritePharmacy>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.FavoritePharmacies)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Pharmacy)
                .WithMany(p => p.UserFavorites)
                .HasForeignKey(e => e.PharmacyId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasIndex(e => new { e.UserId, e.PharmacyId }).IsUnique();
        });

        // MedicineAlternative entity configuration
        builder.Entity<MedicineAlternative>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Reason).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            
            entity.HasOne(e => e.Medicine)
                .WithMany(m => m.Alternatives)
                .HasForeignKey(e => e.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.AlternativeMedicine)
                .WithMany(m => m.AlternativeTo)
                .HasForeignKey(e => e.AlternativeMedicineId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasIndex(e => new { e.MedicineId, e.AlternativeMedicineId }).IsUnique();
        });
    }
}
