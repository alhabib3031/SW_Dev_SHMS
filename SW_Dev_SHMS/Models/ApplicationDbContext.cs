using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SW_Dev_SHMS.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DormManager> DormManager { get; set; }
        public DbSet<DormStudent> DormStudent { get; set; }
        public DbSet<Hostel> Hostel { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Request> Request { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. إعداد العلاقة بين Notification و DormStudent
            // بنخلي الـ OnDelete تكون Restrict عشان نمنع الحذف المتسلسل
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Student)
                .WithMany(s => s.Notification) // 'Notification' هو اسم المجموعة في DormStudent
                .HasForeignKey(n => n.StudentId)
                .OnDelete(DeleteBehavior.Restrict); // <--- التغيير المهم هنا

            // 2. إعداد العلاقة بين Notification و DormManager
            // برضه بنخلي الـ OnDelete تكون Restrict
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Manager)
                .WithMany(m => m.Notification) // 'Notification' هو اسم المجموعة في DormManager
                .HasForeignKey(n => n.ManagerId)
                .OnDelete(DeleteBehavior.Restrict); // <--- التغيير المهم هنا

            // 3. إعداد العلاقة بين DormStudent و DormManager
            // دي غالبًا هي اللي بتسبب المشكلة بشكل غير مباشر.
            // أنت عندك خاصية DormManager في DormStudent بدون مفتاح خارجي صريح.
            // EF Core هيستنتج المفتاح ده، لكن الأفضل نحدد سلوك الحذف
            modelBuilder.Entity<DormStudent>()
                .HasOne(ds => ds.DormManager)
                .WithMany(dm => dm.DormStudents)
                // لو عندك خاصية مفتاح أجنبي صريح في DormStudent للمدير (زي ManagerId), استخدمها هنا:
                // .HasForeignKey(ds => ds.ManagerId)
                .OnDelete(DeleteBehavior.Restrict); // <--- التغيير المهم هنا أيضاً

            // متنساش تستدعي الـ base.OnModelCreating
            base.OnModelCreating(modelBuilder);
        }
    }
}
