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
    public class SocialhelparticlesController : ControllerBase
    {
        private readonly PostgresContext _context;

        public SocialhelparticlesController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Socialhelparticles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Socialhelparticle>>> GetSocialhelparticles()
        {
            return await _context.Socialhelparticles.ToListAsync();
        }

        // GET: api/Socialhelparticles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Socialhelparticle>> GetSocialhelparticle(int id)
        {
            var socialhelparticle = await _context.Socialhelparticles.FindAsync(id);

            if (socialhelparticle == null)
            {
                return NotFound();
            }

            return socialhelparticle;
        }

        // POST: api/Socialhelparticles
        [HttpPost]
        public async Task<ActionResult<Socialhelparticle>> PostSocialhelparticle(Socialhelparticle socialhelparticle)
        {
            _context.Socialhelparticles.Add(socialhelparticle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSocialhelparticle), new { id = socialhelparticle.Id }, socialhelparticle);
        }

        // PUT: api/Socialhelparticles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSocialhelparticle(int id, Socialhelparticle socialhelparticle)
        {
            if (id != socialhelparticle.Id)
            {
                return BadRequest();
            }

            _context.Entry(socialhelparticle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SocialhelparticleExists(id))
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

        // DELETE: api/Socialhelparticles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSocialhelparticle(int id)
        {
            var socialhelparticle = await _context.Socialhelparticles.FindAsync(id);
            if (socialhelparticle == null)
            {
                return NotFound();
            }

            _context.Socialhelparticles.Remove(socialhelparticle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SocialhelparticleExists(int id)
        {
            return _context.Socialhelparticles.Any(e => e.Id == id);
        }
    }
}
