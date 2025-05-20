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
    public class TestimagesController : ControllerBase
    {
        private readonly PostgresContext _context;

        public TestimagesController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Testimages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Testimage>>> GetTestimages()
        {
            return await _context.Testimages.ToListAsync();
        }

        // GET: api/Testimages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Testimage>> GetTestimage(int id)
        {
            var testimage = await _context.Testimages.FindAsync(id);

            if (testimage == null)
            {
                return NotFound();
            }

            return testimage;
        }

        // POST: api/Testimages
        [HttpPost]
        public async Task<ActionResult<Testimage>> PostTestimage(Testimage testimage)
        {
            _context.Testimages.Add(testimage);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTestimage), new { id = testimage.Id }, testimage);
        }

        // PUT: api/Testimages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTestimage(int id, Testimage testimage)
        {
            if (id != testimage.Id)
            {
                return BadRequest();
            }

            _context.Entry(testimage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestimageExists(id))
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

        // DELETE: api/Testimages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestimage(int id)
        {
            var testimage = await _context.Testimages.FindAsync(id);
            if (testimage == null)
            {
                return NotFound();
            }

            _context.Testimages.Remove(testimage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TestimageExists(int id)
        {
            return _context.Testimages.Any(e => e.Id == id);
        }
    }
}
