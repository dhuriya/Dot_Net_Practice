using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.App.Domain;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.App.Data
{
    public class ECommerceDBContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;

        public ECommerceDBContext(DbContextOptions<ECommerceDBContext> options) : base(options)
        {
            
        }
        //This method is called by EF Core to build model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Set default schema for all table this DbContext
            modelBuilder.HasDefaultSchema("sales");   
            // Tables will be created under schema: sales.<TableName>
            // 2. Configure global decimal precision for all decimal properties
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Get all decimal and nullable decimal properties for this entity type
                var decimalProperties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?));
                foreach (var property in decimalProperties)
                {
                    property.SetPrecision(18);   // Total digits = 18
                    property.SetScale(2);        // Digits after decimal = 2 (e.g., 1234567890123.45)
                }
            }
            // 3. Configure global max length for all string properties
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Get all string properties for this entity type
                var stringProperties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(string));
                foreach (var property in stringProperties)
                {
                    // Only apply default length if not already configured
                    if (property.GetMaxLength() == null)
                    {
                        property.SetMaxLength(200);   // Default to varchar(200)
                    }
                }
            }
             // 4. Configure global Enum-to-String conversion for all enum properties
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Get all enum and nullable enum properties
                var enumProperties = entityType.GetProperties()
                    .Where(p => p.ClrType.IsEnum ||
                                (Nullable.GetUnderlyingType(p.ClrType)?.IsEnum ?? false));
                foreach (var property in enumProperties)
                {
                    // Get the actual enum type (handles both enum and nullable enum)
                    var enumType = Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType;
                    // Build EnumToStringConverter<EnumType> dynamically using reflection
                    var converterType = typeof(EnumToStringConverter<>)
                        .MakeGenericType(enumType);
                    // Create an instance of the value converter
                    var converter = (ValueConverter)Activator.CreateInstance(converterType)!;
                    // Apply the converter so enum values are stored as strings in the database
                    property.SetValueConverter(converter);
                }
            }
            // 5. Configure global cascade delete behavior for all relationships
            var foreignKeys = modelBuilder.Model.GetEntityTypes()
                .SelectMany(entityType => entityType.GetForeignKeys());
            foreach (var foreignKey in foreignKeys)
            {
                // Restrict delete by default: parent cannot be deleted if children exist
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
            // 6. Configure all string properties to be non-Unicode (varchar instead of nvarchar)
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var stringProperties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(string));
                foreach (var property in stringProperties)
                {
                    // Only set when Unicode is not explicitly set to false
                    if (property.IsUnicode() != false)
                    {
                        property.SetIsUnicode(false);   // Store strings as varchar in the database
                    }
                }
            }
            // 7. Add global shadow properties for CreatedAt and UpdatedAt
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Skip owned types (value objects, complex types)
                if (entityType.IsOwned())
                    continue;
                // Get CLR properties defined on the entity class
                var clrProperties = entityType.ClrType.GetProperties();
                bool hasCreatedAt = clrProperties.Any(p => p.Name == "CreatedAt");
                bool hasUpdatedAt = clrProperties.Any(p => p.Name == "UpdatedAt");
                // Add CreatedAt as a shadow property only if it does not exist in the entity
                if (!hasCreatedAt)
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property<DateTime?>("CreatedAt");
                }
                // Add UpdatedAt as a shadow property only if it does not exist in the entity
                if (!hasUpdatedAt)
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property<DateTime?>("UpdatedAt");
                }
            }
            // 8. Seed initial dummy data for Customers and Products using HasData
            //    This data will be inserted when migrations are applied.
            modelBuilder.Entity<Customer>().HasData(
                new { Id = 1, FullName = "Pranaya Kumar Rout", Email = "pranaya@example.com", IsDeleted = false, CreatedAt = new DateTime(2025, 01, 01), UpdatedAt = new DateTime(2025, 01, 01) },
                new { Id = 2, FullName = "John Doe", Email = "john.doe@example.com", IsDeleted = false, CreatedAt = new DateTime(2025, 01, 01), UpdatedAt = new DateTime(2025, 01, 01) }
            );
            modelBuilder.Entity<Product>().HasData(
                new { Id = 1, Name = "Dell Laptop", Price = 55000.00m, Quantity = 10, IsDeleted = false, CreatedAt = new DateTime(2025, 01, 01), UpdatedAt = new DateTime(2025, 01, 01) },
                new { Id = 2, Name = "Wireless Mouse", Price = 1200.00m, Quantity = 50, IsDeleted = false, CreatedAt = new DateTime(2025, 01, 01), UpdatedAt = new DateTime(2025, 01, 01) },
                new { Id = 3, Name = "Mechanical Keyboard", Price = 3500.00m, Quantity = 25, IsDeleted = false, CreatedAt = new DateTime(2025, 01, 01), UpdatedAt = new DateTime(2025, 01, 01) }
            );
        }
        // 9. Override SaveChanges to automatically set CreatedAt and UpdatedAt values
        public override int SaveChanges()
        {
            // Get all tracked entities that are either newly added or modified
            var changedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified);
            foreach (var entry in changedEntries)
            {
                // Set CreatedAt only when entity is added and has this property
                if (entry.State == EntityState.Added &&
                    entry.Properties.Any(p => p.Metadata.Name == "CreatedAt"))
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                }
                // Set UpdatedAt whenever entity is added or modified and has this property
                if (entry.Properties.Any(p => p.Metadata.Name == "UpdatedAt"))
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }
            }
            // Delegate the actual saving of changes to the base DbContext implementation
            return base.SaveChanges();
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Get all tracked entities that are either newly added or modified
            var changedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified);
            foreach (var entry in changedEntries)
            {
                // Set CreatedAt only when entity is added and has this property
                if (entry.State == EntityState.Added &&
                    entry.Properties.Any(p => p.Metadata.Name == "CreatedAt"))
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                }
                // Set UpdatedAt whenever entity is added or modified and has this property
                if (entry.Properties.Any(p => p.Metadata.Name == "UpdatedAt"))
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }
            }
            // Delegate the actual saving of changes to the base DbContext implementation (async)
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}