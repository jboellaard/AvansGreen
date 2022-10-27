using UI.AvansGreenApp.Security;
using Core.DomainServices.IRepos;
using Infrastructure.AG_EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var connectionString = builder.Configuration.GetConnectionString("AvansGreenDb");
builder.Services.AddDbContext<AvansGreenDbContext>(options => options.UseSqlServer(connectionString).EnableSensitiveDataLogging(true));

var userConnectionString = builder.Configuration.GetConnectionString("AuthDb");
builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(userConnectionString));

builder.Services.AddIdentity<AvansGreenUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    //options.User.RequireUniqueEmail = true;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OnlyStudentsAndUp", policy => policy
        .RequireAuthenticatedUser()
        .RequireClaim("UserType", new string[] { "Student", "Admin" }));

    options.AddPolicy("OnlyCanteenEmployeesAndUp", policy => policy
        .RequireAuthenticatedUser()
        .RequireClaim("UserType", new string[] { "CanteenEmployee", "Admin" }));
});
builder.Services.AddScoped<AuthDbSeed>();
builder.Services.AddScoped<ICanteenEmployeeRepository, CanteenEmployeeEFRepository>();
builder.Services.AddScoped<ICanteenRepository, CanteenEFRepository>();
builder.Services.AddScoped<IPacketRepository, PacketEFRepository>();
builder.Services.AddScoped<IProductRepository, ProductEFRepository>();
builder.Services.AddScoped<IStudentRepository, StudentEFRepository>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

await SeedData(app);

app.Run();

async Task SeedData(IApplicationBuilder app)
{
    var seedData = app.ApplicationServices
        .CreateScope().ServiceProvider
        .GetRequiredService<AuthDbSeed>();

    await seedData.EnsurePopulated();
}
