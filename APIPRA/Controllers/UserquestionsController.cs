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
    public class UserquestionsController : ControllerBase
    {
        private readonly PostgresContext _context;

        public UserquestionsController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Userquestions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Userquestion>>> GetUserquestions()
        {
            return await _context.Userquestions.ToListAsync();
        }

        // GET: api/Userquestions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Userquestion>> GetUserquestion(int id)
        {
            var userquestion = await _context.Userquestions.FindAsync(id);

            if (userquestion == null)
            {
                return NotFound();
            }

            return userquestion;
        }

        // POST: api/Userquestions
        [HttpPost]
        public async Task<ActionResult<Userquestion>> PostUserquestion(Userquestion userquestion)
        {
            _context.Userquestions.Add(userquestion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserquestion), new { id = userquestion.Id }, userquestion);
        }

        // PUT: api/Userquestions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserquestion(int id, Userquestion userquestion)
        {
            if (id != userquestion.Id)
            {
                return BadRequest();
            }

            _context.Entry(userquestion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserquestionExists(id))
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

        // DELETE: api/Userquestions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserquestion(int id)
        {
            var userquestion = await _context.Userquestions.FindAsync(id);
            if (userquestion == null)
            {
                return NotFound();
            }

            _context.Userquestions.Remove(userquestion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserquestionExists(int id)
        {
            return _context.Userquestions.Any(e => e.Id == id);
        }
    }
}
