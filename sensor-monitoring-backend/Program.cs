using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using sensor_monitoring_backend.Data;
using sensor_monitoring_backend.Hubs;
using sensor_monitoring_backend.Services;
using System.Data.Entity;
using System;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json.Serialization;
using System.Text.Json;
using sensor_monitoring_backend.Infrastructure.ApplicationSettings;
using sensor_monitoring_backend.Infrastructure.Middlewares;
using sensor_monitoring_backend.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<Main>(builder.Configuration.GetSection("AppSettings"));

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173", "http://185.97.119.141:5299", "http://185.97.119.141:5173", "http://185.97.119.141", "https://sensorsonline.ir").AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); ;
                      });
});



//builder.Services.AddSignalR();

var provider = builder.Services.AddTransient<TcpConnection>()
    .AddDbContext<DatabaseContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")))
    .BuildServiceProvider();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.Urls.Add("http://185.97.119.141:5299");
app.Urls.Add("http://127.0.0.1:5299");

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.MapControllers();
app.UseCors(MyAllowSpecificOrigins);
app.UseMiddleware<JwtMiddleware>();
//app.MapHub<SensorHub>("/sensor");


var tcpConnection = provider.GetService<TcpConnection>();
Thread TcpConnectionThread = new Thread(() =>
{
    tcpConnection?.ListenForConnection();
});
TcpConnectionThread.Start();


using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetService<DatabaseContext>();
        if (context == null)
        {
            throw new Exception("Injected DatabaseContext is null");
        }
        await context.Database.MigrateAsync();
        if (!context.Users.Any())
        {
            User user = new User()
            {
                FirstName = "Alireza",
                LastName = "Abdollahi",
                Password = "@Qwerty123",
                UserName = "Alireza013013",
                Role = "Admin"
            };
            context?.Users.Add(user);
            context?.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error : {0}", ex);
    }
}



app.Run();
