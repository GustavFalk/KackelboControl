using KackelboControl_API.Repository;
using KackelboControl_API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IArduinoService, ArduinoService>();
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();

builder.Services.AddDbContext<KackelboControlDbContext>(options =>
  options.UseSqlite(builder.Configuration.GetConnectionString("KackelboControlDbConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
