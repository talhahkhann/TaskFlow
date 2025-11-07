using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using Serilog;
using TaskFlow.Services;

var builder = WebApplication.CreateBuilder(args);
//Serilog
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));
builder.Configuration.AddJsonFile("serilog.json", optional: true);

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    .EnableSensitiveDataLogging()
    .LogTo(Console.WriteLine, LogLevel.Error));
builder.WebHost.UseUrls("http://0.0.0.0:8080");

// Add Controllers
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DI Services
builder.Services.AddScoped<IAuthService, AuthService>();


var app = builder.Build();

//  Configure Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
