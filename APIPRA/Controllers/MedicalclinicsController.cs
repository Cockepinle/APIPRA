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
    public class MedicalclinicsController : ControllerBase
    {
        private readonly PostgresContext _context;

        public MedicalclinicsController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Medicalclinics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicalclinic>>> GetMedicalclinics()
        {
            return await _context.Medicalclinics.ToListAsync();
        }

        // GET: api/Medicalclinics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Medicalclinic>> GetMedicalclinic(int id)
        {
            var medicalclinic = await _context.Medicalclinics.FindAsync(id);

            if (medicalclinic == null)
            {
                return NotFound();
            }

            return medicalclinic;
        }

        // POST: api/Medicalclinics
        [HttpPost]
        public async Task<ActionResult<Medicalclinic>> PostMedicalclinic(Medicalclinic medicalclinic)
        {
            _context.Medicalclinics.Add(medicalclinic);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMedicalclinic), new { id = medicalclinic.Id }, medicalclinic);
        }

        // PUT: api/Medicalclinics/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicalclinic(int id, Medicalclinic medicalclinic)
        {
            if (id != medicalclinic.Id)
            {
                return BadRequest();
            }

            _context.Entry(medicalclinic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicalclinicExists(id))
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

        // DELETE: api/Medicalclinics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalclinic(int id)
        {
            var medicalclinic = await _context.Medicalclinics.FindAsync(id);
            if (medicalclinic == null)
            {
                return NotFound();
            }

            _context.Medicalclinics.Remove(medicalclinic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicalclinicExists(int id)
        {
            return _context.Medicalclinics.Any(e => e.Id == id);
        }
    }
}
