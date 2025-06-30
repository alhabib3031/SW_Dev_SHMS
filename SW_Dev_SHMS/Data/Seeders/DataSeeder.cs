using Microsoft.AspNetCore.Identity;
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

        public DataSeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Seeds all initial data including roles and default manager.
        /// Should be called during application startup.
        /// </summary>
        public async Task SeedAllAsync()
        {
            await SeedRolesAsync();
            await SeedDefaultManagerAsync();
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
    }
}