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
                .Include(p => p.User) // Если есть навигационное свойство
                .ToListAsync();
        }

        // GET: api/Forumposts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Forumpost>> GetById(int id)
        {
            var post = await _context.Forumposts.FindAsync(id);

            if (post == null)
                return NotFound();

            return post;
        }

        // POST: api/Forumposts
       [HttpPost]
        public async Task<ActionResult<Forumpost>> Create([FromBody] Forumpost post)
        {
        try
        {
            if (post.UserId == null)
                return BadRequest("UserId обязателен.");
        
            var userExists = await _context.Users.AnyAsync(u => u.Id == post.UserId);
            if (!userExists)
                return BadRequest("UserId не существует.");
        
            if (post.CreatedAt == default)
                post.CreatedAt = DateTime.UtcNow;
        
            // Очистка навигационного свойства
            post.User = null;
        
            _context.Forumposts.Add(post);
            await _context.SaveChangesAsync();
        
            return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
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
