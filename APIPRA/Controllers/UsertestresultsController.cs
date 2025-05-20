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
    public class UsertestresultsController : ControllerBase
    {
        private readonly PostgresContext _context;

        public UsertestresultsController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Usertestresults
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usertestresult>>> GetUsertestresults()
        {
            return await _context.Usertestresults.ToListAsync();
        }

        // GET: api/Usertestresults/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usertestresult>> GetUsertestresult(int id)
        {
            var usertestresult = await _context.Usertestresults.FindAsync(id);

            if (usertestresult == null)
                return NotFound();

            return usertestresult;
        }

        // POST: api/Usertestresults
        [HttpPost]
        public async Task<ActionResult<Usertestresult>> PostUsertestresult(Usertestresult usertestresult)
        {
            _context.Usertestresults.Add(usertestresult);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsertestresult), new { id = usertestresult.Id }, usertestresult);
        }

        // PUT: api/Usertestresults/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsertestresult(int id, Usertestresult usertestresult)
        {
            if (id != usertestresult.Id)
                return BadRequest();

            _context.Entry(usertestresult).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsertestresultExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Usertestresults/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsertestresult(int id)
        {
            var usertestresult = await _context.Usertestresults.FindAsync(id);
            if (usertestresult == null)
                return NotFound();

            _context.Usertestresults.Remove(usertestresult);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsertestresultExists(int id)
        {
            return _context.Usertestresults.Any(e => e.Id == id);
        }
    }
}
