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
    public class MedicalarticlesController : ControllerBase
    {
        private readonly PostgresContext _context;

        public MedicalarticlesController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Medicalarticles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicalarticle>>> GetMedicalarticles()
        {
            return await _context.Medicalarticles.ToListAsync();
        }

        // GET: api/Medicalarticles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Medicalarticle>> GetMedicalarticle(int id)
        {
            var medicalarticle = await _context.Medicalarticles.FindAsync(id);

            if (medicalarticle == null)
            {
                return NotFound();
            }

            return medicalarticle;
        }

        // POST: api/Medicalarticles
        [HttpPost]
        public async Task<ActionResult<Medicalarticle>> PostMedicalarticle(Medicalarticle medicalarticle)
        {
            _context.Medicalarticles.Add(medicalarticle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMedicalarticle), new { id = medicalarticle.Id }, medicalarticle);
        }

        // PUT: api/Medicalarticles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicalarticle(int id, Medicalarticle medicalarticle)
        {
            if (id != medicalarticle.Id)
            {
                return BadRequest();
            }

            _context.Entry(medicalarticle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicalarticleExists(id))
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

        // DELETE: api/Medicalarticles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalarticle(int id)
        {
            var medicalarticle = await _context.Medicalarticles.FindAsync(id);
            if (medicalarticle == null)
            {
                return NotFound();
            }

            _context.Medicalarticles.Remove(medicalarticle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicalarticleExists(int id)
        {
            return _context.Medicalarticles.Any(e => e.Id == id);
        }
    }
}
