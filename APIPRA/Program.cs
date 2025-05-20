using Microsoft.EntityFrameworkCore;
using APIPRA.Models;

var builder = WebApplication.CreateBuilder(args);

// Получаем порт из переменной окружения (если нет — 8080 по умолчанию)
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

// Настраиваем Kestrel
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔧 Подключаем контекст базы данных
builder.Services.AddDbContext<PostgresContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Добавляем CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// ✅ Включаем CORS
app.UseCors();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
