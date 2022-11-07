using Core.DomainServices.IRepos;
using Core.DomainServices.IServices;
using Core.DomainServices.Services;
using Infrastructure.AG_EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using UI.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddScoped<IPacketRepository, PacketEFRepository>()
    .AddScoped<IStudentRepository, StudentEFRepository>()
    .AddScoped<ICanteenEmployeeRepository, CanteenEmployeeEFRepository>()
    .AddScoped<IProductRepository, ProductEFRepository>()
    .AddScoped<IPacketService, PacketService>()
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
