using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SW_Dev_SHMS.Data.Entities;

namespace SW_Dev_SHMS.Data.Seeders
{
    /// <summary>
    /// DataSeeder class responsible for seeding initial data into the database.
    /// Creates default roles and administrative users for the application.
    /// Designed for school project initialization.
    /// </summary>
    public class DataSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public DataSeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        /// <summary>
        /// Seeds all initial data including roles and default manager.
        /// Should be called during application startup.
        /// </summary>
        public async Task SeedAllAsync()
        {
            await SeedRolesAsync();
            await SeedDefaultManagerAsync();
            await SeedHostelsAndRoomsAsync();
        }

        /// <summary>
        /// Creates the Manager and Student roles if they don't exist.
        /// </summary>
        private async Task SeedRolesAsync()
        {
            string[] roles = { "Manager", "Student" };

            foreach (string role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        /// <summary>
        /// Creates a default manager user for initial system access.
        /// Only creates if no manager exists in the system.
        /// </summary>
        private async Task SeedDefaultManagerAsync()
        {
            const string defaultEmail = "admin@hostel.com";
            const string defaultPassword = "Admin@123";

            // Check if manager already exists
            if (await _userManager.FindByEmailAsync(defaultEmail) == null)
            {
                var defaultManager = new Manager
                {
                    UserName = defaultEmail,
                    Email = defaultEmail,
                    EmailConfirmed = true,
                    FirstName = "System",
                    LastName = "Administrator"
                };

                // Create the manager user
                var result = await _userManager.CreateAsync(defaultManager, defaultPassword);

                if (result.Succeeded)
                {
                    // Assign Manager role
                    await _userManager.AddToRoleAsync(defaultManager, "Manager");
                }
            }
        }

        /// <summary>
        /// Seeds hostels and rooms for the accommodation system.
        /// Creates sample hostels with rooms assigned to the default manager.
        /// </summary>
        private async Task SeedHostelsAndRoomsAsync()
        {
            // Check if hostels already exist
            if (await _context.Hostels.AnyAsync())
            {
                return; // Data already seeded
            }

            // Get the default manager
            var defaultManager = await _context.Managers
                .FirstOrDefaultAsync(m => m.Email == "admin@hostel.com");

            if (defaultManager == null)
            {
                return; // Manager not found, skip hostel seeding
            }

            // Create sample hostels
            var hostels = new List<Hostel>
            {
                new Hostel
                {
                    Name = "مجمع السكن الشمالي - طلاب",
                    Address = "الحرم الجامعي الشمالي، ",
                    DormManager = defaultManager,
                },
                new Hostel
                {
                    Name = "مجمع السكن الجنوبي - طالبات",
                    Address = "الحرم الجامعي الجنوبي، ",
                    DormManager = defaultManager,
                },
                new Hostel
                {
                    Name = "مجمع السكن الشرقي - طلاب",
                    Address = "الحرم الجامعي الشرقي، ",
                    DormManager = defaultManager,
                },
                new Hostel
                {
                    Name = "مجمع السكن الغربي - طالبات",
                    Address = "الحرم الجامعي الغربي، ",
                    DormManager = defaultManager,
                },
                new Hostel
                {
                    Name = "مجمع السكن المركزي - متقدم",
                    Address = "وسط الحرم الجامعي، ",
                    DormManager = defaultManager,
                }
            };

            // Add hostels to context
            _context.Hostels.AddRange(hostels);
            await _context.SaveChangesAsync();

            // Create rooms for each hostel
            var rooms = new List<Room>();

            foreach (var hostel in hostels)
            {
                // Create rooms for each hostel (3 floors, 10 rooms per floor)
                for (int floor = 1; floor <= 3; floor++)
                {
                    for (int roomNum = 1; roomNum <= 10; roomNum++)
                    {
                        string roomNumber = $"{floor}{roomNum:D2}"; // Format: 101, 102, 201, 202, etc.
                        
                        rooms.Add(new Room
                        {
                            RoomNumber = roomNumber,
                            IsAvailable = true,
                            Hostel = hostel
                        });
                    }
                }

                // Add some special rooms (single occupancy, maintenance, etc.)
                rooms.AddRange(new[]
                {
                    new Room
                    {
                        RoomNumber = "G01", // Ground floor room
                        IsAvailable = true,
                        Hostel = hostel
                    },
                    new Room
                    {
                        RoomNumber = "G02", // Ground floor room
                        IsAvailable = true,
                        Hostel = hostel
                    },
                    new Room
                    {
                        RoomNumber = "401", // Top floor room
                        IsAvailable = false, // Some rooms unavailable for maintenance
                        Hostel = hostel
                    },
                    new Room
                    {
                        RoomNumber = "402", // Top floor room
                        IsAvailable = true,
                        Hostel = hostel
                    }
                });
            }

            // Add all rooms to context
            _context.Rooms.AddRange(rooms);
            await _context.SaveChangesAsync();

            // Optionally, mark some rooms as occupied (for testing purposes)
            var someRooms = await _context.Rooms
                .Where(r => r.RoomNumber.EndsWith("01") || r.RoomNumber.EndsWith("02"))
                .Take(10)
                .ToListAsync();

            foreach (var room in someRooms.Take(5)) // Mark first 5 as unavailable
            {
                room.IsAvailable = false;
            }

            await _context.SaveChangesAsync();
        }
    }
}