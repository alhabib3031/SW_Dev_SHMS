using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SW_Dev_SHMS.Data.Entities;
using SW_Dev_SHMS.Models.Entities;

namespace SW_Dev_SHMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Manager> Managers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Hostel> Hostels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
                        // 1. One-to-One: Student <-> Request
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Student)
                .WithOne(s => s.Request)
                .HasForeignKey<Request>("StudentId")
                .OnDelete(DeleteBehavior.Restrict); // Changed to Restrict

            // 2. One-to-One: Request <-> Payment  
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Request)
                .WithOne()
                .HasForeignKey<Payment>("RequestId")
                .OnDelete(DeleteBehavior.Cascade);

            // 3. One-to-One: Hostel <-> Manager
            modelBuilder.Entity<Hostel>()
                .HasOne(h => h.DormManager)
                .WithOne(m => m.Hostel)
                .HasForeignKey<Hostel>("ManagerId")
                .OnDelete(DeleteBehavior.SetNull);

            // 4. Many-to-One: Student -> Room
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Room)
                .WithMany(r => r.Students)
                .HasForeignKey("RoomId")
                .OnDelete(DeleteBehavior.SetNull);

            // 5. Many-to-One: Room -> Hostel
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Hostel)
                .WithMany(h => h.Rooms)
                .HasForeignKey("HostelId")
                .OnDelete(DeleteBehavior.Cascade);

            // 6. Many-to-One: Request -> Hostel
            modelBuilder.Entity<Request>()
                .HasOne(r => r.PreferredHostel)
                .WithMany()
                .HasForeignKey("PreferredHostelId")
                .OnDelete(DeleteBehavior.SetNull);

            // 7. Many-to-One: Notification -> Student
            // Changed to Restrict to avoid cascade conflicts
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Student)
                .WithMany(s => s.Notifications)
                .HasForeignKey("StudentId")
                .OnDelete(DeleteBehavior.Restrict);

            // 8. Many-to-One: Notification -> Manager
            // Changed to Restrict to avoid cascade conflicts
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Manager)
                .WithMany(m => m.Notifications)
                .HasForeignKey("ManagerId")
                .OnDelete(DeleteBehavior.Restrict);

            // 9. Many-to-One: Payment -> Student
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Student)
                .WithMany(s => s.Payments)
                .HasForeignKey("StudentId")
                .OnDelete(DeleteBehavior.Restrict); // Changed to Restrict
        }
    }
}