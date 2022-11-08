using Core.DomainServices.IRepos;
using Infrastructure.AG_EF;
using Microsoft.EntityFrameworkCore;
using UI.GraphQLService.GraphQL;
using UI.GraphQLService.IServices;
using UI.GraphQLService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddScoped<IPacketsGraphQLService, PacketsGraphQLService>()
    .AddScoped<IPacketRepository, PacketEFRepository>()
    .AddScoped<AvansGreenDbSeed>()
    .AddDbContext<AvansGreenDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AvansGreenDb")))
    .AddScoped<PacketQuery>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<PacketQuery>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();


var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL("/graphql/packets");
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    await SeedDatabase();
}

app.Run();

// Add dummy data to the databases
async Task SeedDatabase()
{
    using var scope = app.Services.CreateScope();
    var dbSeeder = scope.ServiceProvider.GetRequiredService<AvansGreenDbSeed>();
    await dbSeeder.EnsurePopulated();
}
