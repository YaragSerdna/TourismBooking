using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourismBooking.Domain.Entities;

namespace TourismBooking.Infrastructure.Persistence
{
    public class TourismBookingDbContext : DbContext
    {
        public TourismBookingDbContext(DbContextOptions<TourismBookingDbContext> options) : base(options)
        {
        }

        public DbSet<Experience> Experiences => Set<Experience>();

        public DbSet<Booking> Bookings => Set<Booking>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureExperience(modelBuilder);
            ConfigureBooking(modelBuilder);
        }

        private static void ConfigureExperience(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Experience>();

            entity.ToTable("Experiences", table =>
            {
                table.HasCheckConstraint("CK_Experiences_PricePerPerson_Positive", "[PricePerPerson] > 0");
                table.HasCheckConstraint("CK_Experiences_MaxCapacity_Positive", "[MaxCapacity] > 0");
            });

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name).IsRequired().HasMaxLength(150);

            entity.Property(x => x.Description).IsRequired().HasMaxLength(2000);

            entity.Property(x => x.Destination).IsRequired().HasMaxLength(150);

            entity.Property(x => x.PricePerPerson).HasPrecision(18, 2);

            entity.Property(x => x.MaxCapacity).IsRequired();

            entity.Property(x => x.Status).IsRequired();

            entity.Property(x => x.CreatedAt).IsRequired();

            entity.Property(x => x.UpdatedAt);

            entity.HasMany(x => x.Bookings).WithOne(x => x.Experience).HasForeignKey(x => x.ExperienceId).OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.Status);
        }

        private static void ConfigureBooking(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Booking>();

            entity.ToTable("Bookings", table =>
            {
                table.HasCheckConstraint("CK_Bookings_PassengerCount_Positive", "[PassengerCount] > 0");
                table.HasCheckConstraint("CK_Bookings_TotalAmount_NonNegative", "[TotalAmount] >= 0");
            });

            entity.HasKey(x => x.Id);

            entity.Property(x => x.ActivityDate).IsRequired().HasColumnType("date");

            entity.Property(x => x.PassengerCount).IsRequired();

            entity.Property(x => x.CustomerName).IsRequired().HasMaxLength(150);

            entity.Property(x => x.CustomerEmail).IsRequired().HasMaxLength(254);

            entity.Property(x => x.TotalAmount).HasPrecision(18, 2);

            entity.Property(x => x.Status).IsRequired();

            entity.Property(x => x.CreatedAt).IsRequired();

            entity.Property(x => x.UpdatedAt);

            entity.HasIndex(x => new { x.ExperienceId, x.ActivityDate, x.Status });
        }
    }
}
