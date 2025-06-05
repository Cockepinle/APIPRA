using Microsoft.EntityFrameworkCore;
using APIPRA.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Получаем порт из переменной окружения (если нет — 8080 по умолчанию)
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

// Настраиваем Kestrel
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Добавьте это в начало метода Main или ConfigureServices
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", false);
// 🔧 Подключаем контекст базы данных
builder.Services.AddDbContext<PostgresContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging());
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
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseSwagger();
app.UseSwaggerUI();

// ✅ Включаем CORS
app.UseCors();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
