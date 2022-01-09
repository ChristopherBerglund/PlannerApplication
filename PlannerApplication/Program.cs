using Microsoft.EntityFrameworkCore;
using PlannerApplication.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
string _connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");
string _identitystring = builder.Configuration.GetConnectionString("IdentityConnection");



//builder.Services.AddDbContext<PlannerApplicationContext>(options =>
//    options.UseSqlServer(_identitystring));builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<PlannerApplicationContext>();// Add services to the container.




builder.Services.AddDbContext<PlannerApplicationContext>(options =>
            options
            .UseMySql(_identitystring, ServerVersion.AutoDetect(_identitystring))); 
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
.AddEntityFrameworkStores<PlannerApplicationContext>();// Add services to the container.



builder.Services.AddDbContext<PlannerContext>(Options =>
            Options
                .UseMySql(_connectionstring, ServerVersion.AutoDetect(_connectionstring))
            );
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

app.Run();
