using Microsoft.EntityFrameworkCore;
using Server.Models.Entitys;

namespace Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Account> Accounts { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<PreKey> PreKeys { get; set; }
        public DbSet<AddDeviceData> AddDeviceData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Account
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.HasMany(e => e.Devices)
                      .WithOne(d => d.Account)
                      .HasForeignKey(d => d.AccountId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Device
            modelBuilder.Entity<Device>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.AccountId).IsRequired();
                entity.Property(e => e.SPK).IsRequired();
                entity.Property(e => e.Signature).IsRequired();
                entity.HasMany(e => e.PreKeys)
                      .WithOne(p => p.Device)
                      .HasForeignKey(p => p.DeviceId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // PreKey
            modelBuilder.Entity<PreKey>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.DeviceId).IsRequired();
                entity.Property(e => e.PK).IsRequired();
                entity.Property(e => e.PKSignature).IsRequired();
                entity.Property(e => e.IsUsed).IsRequired();
            });
        }
    }
}
