using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AppointmentWebApp.Data;
using AppointmentWebApp.Models;
using AppointmentWebApp.Services;
using AppointmentWebApp.Hubs;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));



builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<AppointmentService>();



// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}


app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<NotificationHub>("/notificationHub");


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});


app.MapRazorPages();



using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Doctor", "Patient" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    string email = "admin@admin.com";
    string password = "Test1234,";


    var existingUser = await userManager.FindByEmailAsync(email);
    if (existingUser != null)
    {
        await userManager.AddToRoleAsync(existingUser, "Admin");
    }
    else
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
        else
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine(error.Description);
            }
        }
    }
}


app.Run();
