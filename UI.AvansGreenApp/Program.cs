using Core.DomainServices.IRepos;
using Core.DomainServices.IServices;
using Core.DomainServices.Services;
using Infrastructure.AG_EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UI.AvansGreenApp.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services
    .AddScoped<ICanteenEmployeeRepository, CanteenEmployeeEFRepository>()
    .AddScoped<IPacketRepository, PacketEFRepository>()
    .AddScoped<IProductRepository, ProductEFRepository>()
    .AddScoped<IStudentRepository, StudentEFRepository>()
    .AddScoped<IPacketService, PacketService>()
    .AddScoped<AvansGreenDbSeed>()
    .AddScoped<AuthDbSeed>()
    // EF database
    .AddDbContext<AvansGreenDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AvansGreenDb")))
    // Identity database    
    .AddDbContext<AuthDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AuthDb")))
    .AddIdentity<AvansGreenUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

// Add policies (authentication enabled by Identity)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OnlyStudentsAndUp", policy => policy
        .RequireAuthenticatedUser()
        .RequireClaim("UserType", new string[] { "Student", "Admin" }));

    options.AddPolicy("OnlyCanteenEmployeesAndUp", policy => policy
        .RequireAuthenticatedUser()
        .RequireClaim("UserType", new string[] { "CanteenEmployee", "Admin" }));
});


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = false;
});

var app = builder.Build();

// Finding issues (temporary, delete later!)
app.UseDeveloperExceptionPage();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{

}
await SeedDatabase();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();

// Add dummy data to authentication database
async Task SeedDatabase()
{
    using var scope = app.Services.CreateScope();
    var dbSeeder = scope.ServiceProvider.GetRequiredService<AvansGreenDbSeed>();
    await dbSeeder.EnsurePopulated();

    var authDbSeeder = scope.ServiceProvider.GetRequiredService<AuthDbSeed>();
    await authDbSeeder.EnsurePopulated();
}