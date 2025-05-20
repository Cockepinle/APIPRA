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
    public class LegalarticlesController : ControllerBase
    {
        private readonly PostgresContext _context;

        public LegalarticlesController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Legalarticles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Legalarticle>>> GetLegalarticles()
        {
            return await _context.Legalarticles.ToListAsync();
        }

        // GET: api/Legalarticles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Legalarticle>> GetLegalarticle(int id)
        {
            var legalarticle = await _context.Legalarticles.FindAsync(id);

            if (legalarticle == null)
            {
                return NotFound();
            }

            return legalarticle;
        }

        // POST: api/Legalarticles
        [HttpPost]
        public async Task<ActionResult<Legalarticle>> PostLegalarticle(Legalarticle legalarticle)
        {
            _context.Legalarticles.Add(legalarticle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLegalarticle), new { id = legalarticle.Id }, legalarticle);
        }

        // PUT: api/Legalarticles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLegalarticle(int id, Legalarticle legalarticle)
        {
            if (id != legalarticle.Id)
            {
                return BadRequest();
            }

            _context.Entry(legalarticle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LegalarticleExists(id))
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

        // DELETE: api/Legalarticles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLegalarticle(int id)
        {
            var legalarticle = await _context.Legalarticles.FindAsync(id);
            if (legalarticle == null)
            {
                return NotFound();
            }

            _context.Legalarticles.Remove(legalarticle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LegalarticleExists(int id)
        {
            return _context.Legalarticles.Any(e => e.Id == id);
        }
    }
}
