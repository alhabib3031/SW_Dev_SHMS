using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SW_Dev_SHMS.Models
{
    public class ApplicationDbContext : IdentityDbContext<Student>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Manager> DormManager { get; set; }
        public DbSet<Student> DormStudent { get; set; }
        public DbSet<Hostel> Hostel { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Request> Request { get; set; }
        public DbSet<Payment> Payment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. إعداد العلاقة بين Notification و Student
            // بنخلي الـ OnDelete تكون Restrict عشان نمنع الحذف المتسلسل
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Student)
                .WithMany(s => s.Notification) // 'Notification' هو اسم المجموعة في Student
                .HasForeignKey(n => n.StudentId)
                .OnDelete(DeleteBehavior.Restrict); // <--- التغيير المهم هنا

            // 2. إعداد العلاقة بين Notification و Manager
            // برضه بنخلي الـ OnDelete تكون Restrict
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Manager)
                .WithMany(m => m.Notification) // 'Notification' هو اسم المجموعة في Manager
                .HasForeignKey(n => n.ManagerId)
                .OnDelete(DeleteBehavior.Restrict); // <--- التغيير المهم هنا

            // 3. إعداد العلاقة بين Student و Manager
            // دي غالبًا هي اللي بتسبب المشكلة بشكل غير مباشر.
            // أنت عندك خاصية Manager في Student بدون مفتاح خارجي صريح.
            // EF Core هيستنتج المفتاح ده، لكن الأفضل نحدد سلوك الحذف
            modelBuilder.Entity<Student>()
                .HasOne(ds => ds.DormManager)
                .WithMany(dm => dm.DormStudents)
                // لو عندك خاصية مفتاح أجنبي صريح في Student للمدير (زي ManagerId), استخدمها هنا:
                // .HasForeignKey(ds => ds.ManagerId)
                .OnDelete(DeleteBehavior.Restrict); // <--- التغيير المهم هنا أيضاً

            // متنساش تستدعي الـ base.OnModelCreating
            base.OnModelCreating(modelBuilder);
        }
    }
}
