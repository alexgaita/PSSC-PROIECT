using Microsoft.EntityFrameworkCore;
using TakeCommand.Data;
using TakeCommand.Data.Interfaces;
using TakeCommand.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ShopContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));

    options.UseMySql(connectionString, serverVersion)
                 // The following three options help with debugging, but should
                 // be changed or removed for production.
                 .LogTo(Console.WriteLine, LogLevel.Information)
                 .EnableSensitiveDataLogging()
                 .EnableDetailedErrors();
});

builder.Services.AddTransient<IOrderRepository, OrderRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
