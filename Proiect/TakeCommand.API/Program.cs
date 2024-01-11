using Domain.Repositories;
using Domain.Services;
using Domain.Workflows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using TakeCommand.Data;
using TakeCommand.Data.Repositories;
using TakeCommand.Events;

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
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<PlaceOrderWorkflow>();
//TODO add product repository
//TODO add orderProduct repository

builder.Services.AddSingleton<IEventSender, ServiceBusTopicEventSender>();

builder.Services.AddAzureClients(client =>
{
    client.AddServiceBusClient(builder.Configuration.GetConnectionString("ServiceBus"));
});



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
