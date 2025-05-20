using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIPRA.Models;

namespace APIPRA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkarticlesController : ControllerBase
    {
        private readonly PostgresContext _context;

        public WorkarticlesController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Workarticles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Workarticle>>> GetWorkarticles()
        {
            return await _context.Workarticles.ToListAsync();
        }

        // GET: api/Workarticles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Workarticle>> GetWorkarticle(int id)
        {
            var workarticle = await _context.Workarticles.FindAsync(id);

            if (workarticle == null)
                return NotFound();

            return workarticle;
        }

        // POST: api/Workarticles
        [HttpPost]
        public async Task<ActionResult<Workarticle>> PostWorkarticle(Workarticle workarticle)
        {
            _context.Workarticles.Add(workarticle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWorkarticle), new { id = workarticle.Id }, workarticle);
        }

        // PUT: api/Workarticles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkarticle(int id, Workarticle workarticle)
        {
            if (id != workarticle.Id)
                return BadRequest();

            _context.Entry(workarticle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkarticleExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Workarticles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkarticle(int id)
        {
            var workarticle = await _context.Workarticles.FindAsync(id);
            if (workarticle == null)
                return NotFound();

            _context.Workarticles.Remove(workarticle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorkarticleExists(int id)
        {
            return _context.Workarticles.Any(e => e.Id == id);
        }
    }
}
