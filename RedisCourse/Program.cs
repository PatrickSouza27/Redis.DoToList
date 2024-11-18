using Microsoft.EntityFrameworkCore;
using RedisCourse.Controllers;
using RedisCourse.Infrastructure.Caching;
using RedisCourse.Infrastructure.Caching.Interfaces;
using RedisCourse.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddDbContext<ToDoListDbContext>(x=> x.UseInMemoryDatabase("ToDoListDb"));
builder.Services.AddScoped<ICachingService, CachingService>();


//string connectionString = builder.Configuration.GetConnectionString("Redis"); or use the connection string directly DOCKER
builder.Services.AddStackExchangeRedisCache(x =>
{
    //here you can use the connection string from the appsettings.json (connection Public endpoint)
    //redis-18288.c245.us-east-1-3.ec2.redns.redis-cloud.com:18288 , senha
    x.Configuration = "redis-18288.c245.us-east-1-3.ec2.redns.redis-cloud.com:18288,password=password123###";
    x.InstanceName = "RedisCourse";
});

var app = builder.Build();


app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();