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
    public class MigrationcentersController : ControllerBase
    {
        private readonly PostgresContext _context;

        public MigrationcentersController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Migrationcenters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Migrationcenter>>> GetMigrationcenters()
        {
            return await _context.Migrationcenters.ToListAsync();
        }

        // GET: api/Migrationcenters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Migrationcenter>> GetMigrationcenter(int id)
        {
            var migrationcenter = await _context.Migrationcenters.FindAsync(id);

            if (migrationcenter == null)
            {
                return NotFound();
            }

            return migrationcenter;
        }

        // POST: api/Migrationcenters
        [HttpPost]
        public async Task<ActionResult<Migrationcenter>> PostMigrationcenter(Migrationcenter migrationcenter)
        {
            _context.Migrationcenters.Add(migrationcenter);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMigrationcenter), new { id = migrationcenter.Id }, migrationcenter);
        }

        // PUT: api/Migrationcenters/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMigrationcenter(int id, Migrationcenter migrationcenter)
        {
            if (id != migrationcenter.Id)
            {
                return BadRequest();
            }

            _context.Entry(migrationcenter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MigrationcenterExists(id))
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

        // DELETE: api/Migrationcenters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMigrationcenter(int id)
        {
            var migrationcenter = await _context.Migrationcenters.FindAsync(id);
            if (migrationcenter == null)
            {
                return NotFound();
            }

            _context.Migrationcenters.Remove(migrationcenter);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MigrationcenterExists(int id)
        {
            return _context.Migrationcenters.Any(e => e.Id == id);
        }
    }
}
