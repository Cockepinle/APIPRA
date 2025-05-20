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
    public class LanguagetestsController : ControllerBase
    {
        private readonly PostgresContext _context;

        public LanguagetestsController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Languagetests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Languagetest>>> GetLanguagetests()
        {
            return await _context.Languagetests.ToListAsync();
        }

        // GET: api/Languagetests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Languagetest>> GetLanguagetest(int id)
        {
            var languagetest = await _context.Languagetests.FindAsync(id);

            if (languagetest == null)
            {
                return NotFound();
            }

            return languagetest;
        }

        // POST: api/Languagetests
        [HttpPost]
        public async Task<ActionResult<Languagetest>> PostLanguagetest(Languagetest languagetest)
        {
            _context.Languagetests.Add(languagetest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLanguagetest), new { id = languagetest.Id }, languagetest);
        }

        // PUT: api/Languagetests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLanguagetest(int id, Languagetest languagetest)
        {
            if (id != languagetest.Id)
            {
                return BadRequest();
            }

            _context.Entry(languagetest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LanguagetestExists(id))
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

        // DELETE: api/Languagetests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLanguagetest(int id)
        {
            var languagetest = await _context.Languagetests.FindAsync(id);
            if (languagetest == null)
            {
                return NotFound();
            }

            _context.Languagetests.Remove(languagetest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LanguagetestExists(int id)
        {
            return _context.Languagetests.Any(e => e.Id == id);
        }
    }
}
