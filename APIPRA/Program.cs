using Microsoft.EntityFrameworkCore;
using APIPRA.Models;

var builder = WebApplication.CreateBuilder(args);

// Получаем порт из переменной окружения (если нет — 8080 по умолчанию)
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

// Настраиваем Kestrel, чтобы слушать этот порт
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PostgresContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
