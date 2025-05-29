using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIPRA.Models;

namespace APIPRA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForumpostsController : ControllerBase
    {
        private readonly PostgresContext _context;

        public ForumpostsController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Forumposts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Forumpost>>> GetAll()
        {
            return await _context.Forumposts
                .AsNoTracking()
                .Select(p => new Forumpost
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    Title = p.Title,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    User = null // Исключаем навигационное свойство
                })
                .ToListAsync();
        }

        // GET: api/Forumposts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Forumpost>> GetById(int id)
        {
            var post = await _context.Forumposts
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
                return NotFound();

            return post;
        }

        // POST: api/Forumposts
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreateForumpostDto postDto)
        {
            try
            {
                // Проверка существования пользователя
                var user = await _context.Users.FindAsync(postDto.UserId);
                if (user == null)
                    return BadRequest("Пользователь не найден");

                var post = new Forumpost
                {
                    UserId = postDto.UserId,
                    Title = postDto.Title,
                    Content = postDto.Content
                };

                _context.Forumposts.Add(post);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    post.Id,
                    post.Title,
                    post.Content,
                    post.CreatedAt,
                    Author = user.Name
                });
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database error: {dbEx.InnerException?.Message}");
                return StatusCode(500, new
                {
                    Message = "Ошибка базы данных",
                    Details = dbEx.InnerException?.Message
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return StatusCode(500, new
                {
                    Message = "Внутренняя ошибка сервера",
                    Details = ex.Message
                });
            }
        }

        // PUT: api/Forumposts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Forumpost post)
        {
            if (id != post.Id)
                return BadRequest("ID в URL не совпадает с ID в теле запроса.");

            var exists = await _context.Forumposts.AnyAsync(p => p.Id == id);
            if (!exists)
                return NotFound("Пост не найден.");

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Ошибка при сохранении изменений.");
            }

            return NoContent();
        }

        // DELETE: api/Forumposts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Forumposts.FindAsync(id);
            if (post == null)
                return NotFound();

            _context.Forumposts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
