var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddGraphQLServer()
//    .RegisterDbContext<AvansGreenDbContext>(DbContextKind.Pooled)
//    .AddProjections()
//    .AddFiltering().AddSorting();

//builder.Services.AddScoped<IPacketRepository, PacketEFRepository>()
//    .AddDbContext<AvansGreenDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AvansGreenDb")));



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.MapControllers();

app.Run();
