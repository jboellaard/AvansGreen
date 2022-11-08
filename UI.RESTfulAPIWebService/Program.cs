using Core.DomainServices.IRepos;
using Core.DomainServices.IServices;
using Core.DomainServices.Services;
using Infrastructure.AG_EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using UI.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    })
.AddXmlSerializerFormatters()
.AddXmlDataContractSerializerFormatters();

builder.Services.AddEndpointsApiExplorer()
     .AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "RESTful API for managing reservations of a student", Version = "v1" });
        });

builder.Services
    .AddScoped<IPacketRepository, PacketEFRepository>()
    .AddScoped<IStudentRepository, StudentEFRepository>()
    .AddScoped<ICanteenEmployeeRepository, CanteenEmployeeEFRepository>()
    .AddScoped<IProductRepository, ProductEFRepository>()
    .AddScoped<IPacketService, PacketService>()
    .AddScoped<AvansGreenDbSeed>()
    .AddScoped<AuthDbSeed>()
    .AddDbContext<AvansGreenDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AvansGreenDb")));

builder.Services
    .AddDbContext<AuthDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("AuthDb")))
    .AddIdentity<AvansGreenUser, IdentityRole>(options => options.SignIn.RequireConfirmedEmail = false)
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

//Configure JWT usage.
builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters.ValidateAudience = false;
    options.TokenValidationParameters.ValidateIssuer = false;
    options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["BearerTokens:Key"]));

});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await SeedDatabase();
}

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

// Add dummy data to the databases
async Task SeedDatabase()
{
    using var scope = app.Services.CreateScope();
    var dbSeeder = scope.ServiceProvider.GetRequiredService<AvansGreenDbSeed>();
    await dbSeeder.EnsurePopulated();

    var authDbSeeder = scope.ServiceProvider.GetRequiredService<AuthDbSeed>();
    await authDbSeeder.EnsurePopulated();
}
