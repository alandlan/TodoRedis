using Microsoft.EntityFrameworkCore;
using TodoRedis.Data;
using TodoRedis.Services;
using TodoRedis.Services.Caching;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("TodoDb");

builder.Services.AddDbContext<TodoDbContext>(opts => opts.UseMySql(
    connectionString,
    ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<ICachingService, CachingService>();
builder.Services.AddScoped<ITodoService, TodoService>();

var redisConnectionString = builder.Configuration.GetConnectionString("Redis");

builder.Services.AddStackExchangeRedisCache(opts =>
{
    opts.InstanceName = "TodoRedis";
    opts.Configuration = "localhost:6379";
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

app.UseCors(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
