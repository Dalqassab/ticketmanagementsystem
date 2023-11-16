using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Data;
using TicketManagementSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();    
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
/*using (var scope = app.Services.CreateScope()) { 
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "Technician", "Member" };
    foreach(var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));

    }
}
*/
// This should be just before your user and role seeding logic.

// Move user and role management code here, after app.Run()
async Task InitializeRolesAndAdminUser(IServiceProvider serviceProvider)
{
    using (var scope = serviceProvider.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = new[] { "Admin", "Technician", "Member" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    using (var scope = serviceProvider.CreateScope())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string email = "admin@admin.com";
        string password = "Test123!,";
        string fname = "admin";

        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new ApplicationUser { UserName = email, FirstName=fname, LastName=fname,Email = email, EmailConfirmed = true };

            var createResult = await userManager.CreateAsync(user, password);
            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                // Handle the case when user creation fails
                throw new InvalidOperationException($"Admin user could not be created: {createResult.Errors.FirstOrDefault()?.Description}");
            }
        }
    }
}

// Call the initialization method here. Make sure it's after app.Run() and you await it if your Main method is async
await InitializeRolesAndAdminUser(app.Services);

app.Run();
