﻿using APIPRA.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using APIPRA.Models;

namespace APIPRA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {      
            private readonly PostgresContext _context;

            public AccountController(PostgresContext context)
            {
                _context = context;
            }

            // POST: api/account/register
           [HttpPost("register")]
            public async Task<IActionResult> Register([FromBody] RegisterRequest model)
            {
                if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                    return BadRequest("Email и пароль обязательны.");
            
                try
                {
                    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                    if (existingUser != null)
                        return Conflict("Пользователь с таким Email уже существует.");
            
                    var user = new User
                    {
                        Name = model.Name,
                        Email = model.Email,
                        PasswordHash = HashPassword(model.Password)
                    };
            
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
            
                    return Ok(new { message = "Регистрация прошла успешно." });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка регистрации: {ex.Message}");
                    return StatusCode(500, "Внутренняя ошибка сервера");
                }
            }


            // POST: api/account/login
            [HttpPost("login")]
            public async Task<IActionResult> Login([FromBody] LoginRequest model)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null || user.PasswordHash != HashPassword(model.Password))
                    return Unauthorized("Неверный email или пароль.");

                return Ok(new
                {
                    message = "Успешный вход",
                    user = new { user.Id, user.Name, user.Email }
                });
            }

            private string HashPassword(string password)
            {
                using var sha256 = SHA256.Create();
                var bytes = Encoding.UTF8.GetBytes(password);
                return Convert.ToBase64String(sha256.ComputeHash(bytes));
            }
        }

    
}
