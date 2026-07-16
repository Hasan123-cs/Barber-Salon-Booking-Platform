using BarberSalon.Data;
using BarberSalon.Models;
using BarberSalon.Services;
using BarberSalon.Services.Implements;
using BarberSalon.Services.Interfaces;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;


Env.Load();

var builder = WebApplication.CreateBuilder(args);


// to still work without change all we do for env variable 

builder.Configuration["ConnectionStrings:DefaultConnection"] =Environment.GetEnvironmentVariable("CONNECTION_STRING");

builder.Configuration["Supabase:Url"] =Environment.GetEnvironmentVariable("SUPABASE_URL");

builder.Configuration["Supabase:Key"] =Environment.GetEnvironmentVariable("SUPABASE_KEY");
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IAccountServices, AccountServices>();
builder.Services.AddScoped<IEmployeeServices, EmployeeServices>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 6;

        options.Password.RequireDigit = true;

        options.Password.RequireLowercase = true;
        // since upper case is optional ( i choose this part)
        options.Password.RequireUppercase = false;
        // no special character
        options.Password.RequireNonAlphanumeric = false;
        // unique email
        options.User.RequireUniqueEmail = true;

    })
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddScoped<SupabaseService>();
builder.Services.ConfigureApplicationCookie(options =>
{
    // login for the non authenticated user return also if access denied happen (user enter the admin dashboard)'
    // httponly use for secure from xss attack from java script by reading the cookie 
    // secure session for example by using httponly or cookie.securepolicy (accept only https)
    // ExpireTimeSpan maximum user can login 30 min into our website 
    // SlidingExpiration this mean if user do any activity re add the time example from 1 to 30 if user 
    // click on 15 its now to 45 not 30
    options.LoginPath = "/Account/Login";
    options.Cookie.MaxAge = null;
    options.AccessDeniedPath = "/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.SlidingExpiration = true;
});
// configure session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{

    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // do migration for docker when run 
    Console.WriteLine("START MIGRATION");

    db.Database.Migrate();

    Console.WriteLine("MIGRATION DONE");
    var services = scope.ServiceProvider;
    var serv = scope.ServiceProvider.GetRequiredService<IAccountServices>();

    await serv.SeedRoles(services);
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
// get an hash password to test data (dome testing)



app.Run();
