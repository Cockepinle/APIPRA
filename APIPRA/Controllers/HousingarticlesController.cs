using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIPRA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIPRA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HousingarticlesController : ControllerBase
    {
        private readonly PostgresContext _context;

        public HousingarticlesController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Housingarticles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Housingarticle>>> GetHousingarticles()
        {
            return await _context.Housingarticles.ToListAsync();
        }

        // GET: api/Housingarticles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Housingarticle>> GetHousingarticle(int id)
        {
            var housingarticle = await _context.Housingarticles.FindAsync(id);

            if (housingarticle == null)
            {
                return NotFound();
            }

            return housingarticle;
        }

        // POST: api/Housingarticles
        [HttpPost]
        public async Task<ActionResult<Housingarticle>> PostHousingarticle(Housingarticle housingarticle)
        {
            _context.Housingarticles.Add(housingarticle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHousingarticle), new { id = housingarticle.Id }, housingarticle);
        }

        // PUT: api/Housingarticles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHousingarticle(int id, Housingarticle housingarticle)
        {
            if (id != housingarticle.Id)
            {
                return BadRequest();
            }

            _context.Entry(housingarticle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HousingarticleExists(id))
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

        // DELETE: api/Housingarticles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHousingarticle(int id)
        {
            var housingarticle = await _context.Housingarticles.FindAsync(id);
            if (housingarticle == null)
            {
                return NotFound();
            }

            _context.Housingarticles.Remove(housingarticle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HousingarticleExists(int id)
        {
            return _context.Housingarticles.Any(e => e.Id == id);
        }
    }
}