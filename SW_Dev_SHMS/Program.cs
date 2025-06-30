using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using SW_Dev_SHMS.Models;
using Microsoft.Extensions.DependencyInjection; // Ensure this namespace is included for Identity services  

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.  
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Fix for CS1061: Ensure the correct Identity package is referenced and use AddIdentity instead of AddDefaultIdentity  
builder.Services.AddIdentity<Student, IdentityRole>(options =>
   {
      options.Password.RequireNonAlphanumeric = false;
      options.Password.RequireUppercase = true;
      options.Password.RequireLowercase = true;
      options.Password.RequiredLength = 8;
      options.User.RequireUniqueEmail = true;
      options.SignIn.RequireConfirmedAccount = true;
      options.SignIn.RequireConfirmedEmail = true;
      options.SignIn.RequireConfirmedPhoneNumber = false;
   })
   .AddEntityFrameworkStores<ApplicationDbContext>()
   .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.Configure<FormOptions>(options =>
{
   options.MultipartBodyLengthLimit = 104857600; // 100 MB
});

var app = builder.Build();

// Configure the HTTP request pipeline.  
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.  
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
   name: "default",
   pattern: "{controller=Home}/{action=Index}/{id?}")
   .WithStaticAssets();
app.MapRazorPages();
app.Run();
