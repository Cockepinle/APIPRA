using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIPRA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIPRA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForumrepliesController : ControllerBase
    {
        private readonly PostgresContext _context;

        public ForumrepliesController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Forumreplies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Forumreply>>> GetForumreplies()
        {
            return await _context.Forumreplies.ToListAsync();
        }

        // GET: api/Forumreplies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Forumreply>> GetForumreply(int id)
        {
            var forumreply = await _context.Forumreplies.FindAsync(id);

            if (forumreply == null)
            {
                return NotFound();
            }

            return forumreply;
        }

        // POST: api/Forumreplies
        [HttpPost]
        public async Task<ActionResult<Forumreply>> PostForumreply(Forumreply forumreply)
        {
            _context.Forumreplies.Add(forumreply);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetForumreply), new { id = forumreply.Id }, forumreply);
        }

        // PUT: api/Forumreplies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutForumreply(int id, Forumreply forumreply)
        {
            if (id != forumreply.Id)
            {
                return BadRequest();
            }

            _context.Entry(forumreply).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForumreplyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Forumreplies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForumreply(int id)
        {
            var forumreply = await _context.Forumreplies.FindAsync(id);
            if (forumreply == null)
            {
                return NotFound();
            }

            _context.Forumreplies.Remove(forumreply);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ForumreplyExists(int id)
        {
            return _context.Forumreplies.Any(e => e.Id == id);
        }
    }
}