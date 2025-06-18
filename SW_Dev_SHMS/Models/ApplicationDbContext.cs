using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SW_Dev_SHMS.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        public DbSet<DormManager> DormManager { get; set; }
        public DbSet<DormStudent> DormStudent { get; set; }
        public DbSet<Hostel> Hostel { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Request> Request { get; set; }
        public DbSet<Payment> Payment { get; set; }
    }


}
