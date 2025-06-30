using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using SW_Dev_SHMS.Data;
using SW_Dev_SHMS.Data.Entities;
using SW_Dev_SHMS.Data.Seeders;
using SW_Dev_SHMS.Models;
using SW_Dev_SHMS.Services.DataServices;
using SW_Dev_SHMS.Services.DataServices.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Database context configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseLazyLoadingProxies());

// Identity configuration with ApplicationUser as base class
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password requirements
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 8;
    
    // User requirements
    options.User.RequireUniqueEmail = true;
    
    // Sign-in requirements (relaxed for school project)
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Email sender service
builder.Services.AddSingleton<IEmailSender, EmailSender>();

//SEEDER
builder.Services.AddScoped<DataSeeder>();

// Services 
builder.Services.AddScoped<IStudentDataService, StudentsDataService>();

// File upload configuration
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    try
    {
        logger.LogInformation("Starting database seeding...");
        
        var seeder = services.GetRequiredService<DataSeeder>();
        await seeder.SeedAllAsync();
        
        logger.LogInformation("Database seeding completed successfully");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database: {Message}", ex.Message);
        // Don't throw - let the app continue even if seeding fails
    }
}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Authentication & Authorization middleware
app.UseAuthentication(); // Added - required for Identity
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();