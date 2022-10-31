var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddPooledDbContextFactory<OrderContext>(options => options.UseSqlite("Data Source=Orders.db")
//    .EnableSensitiveDataLogging()).AddLogging(Console.WriteLine);

// Configure JWT usage.
//builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
//{
//    options.TokenValidationParameters.ValidateAudience = false;
//    options.TokenValidationParameters.ValidateIssuer = false;

//});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
