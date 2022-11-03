using Core.DomainServices.IRepos;
using Infrastructure.AG_EF;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
}); ;

builder.Services.AddGraphQLServer()
    .RegisterDbContext<AvansGreenDbContext>(DbContextKind.Pooled)
    .AddProjections()
    .AddFiltering().AddSorting();

builder.Services.AddScoped<IPacketRepository, PacketEFRepository>()
    .AddDbContext<AvansGreenDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AvansGreenDb")));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.MapControllers();

app.Run();
