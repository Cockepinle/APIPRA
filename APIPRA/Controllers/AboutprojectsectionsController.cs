using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIPRA.Models;

namespace APIPRA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AboutprojectsectionsController : ControllerBase
    {
        private readonly PostgresContext _context;

        public AboutprojectsectionsController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Aboutprojectsections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aboutprojectsection>>> GetAll()
        {
            var list = await _context.Aboutprojectsections.ToListAsync();
            return Ok(list);
        }

        // GET: api/Aboutprojectsections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Aboutprojectsection>> Get(int id)
        {
            try
            {
                var item = await _context.Aboutprojectsections.FindAsync(id);
        
                if (item == null)
                    return NotFound();
        
                return Ok(item);
            }
            catch (Exception ex)
            {
                // Возвращаем статус 500 и сообщение ошибки, чтобы понять, что не так
                return StatusCode(500, $"Internal error: {ex.Message}");
            }
        }


        // POST: api/Aboutprojectsections
        [HttpPost]
        public async Task<ActionResult<Aboutprojectsection>> Create(Aboutprojectsection aboutprojectsection)
        {
            _context.Aboutprojectsections.Add(aboutprojectsection);
            await _context.SaveChangesAsync();

            // Возвращаем 201 Created и ссылку на созданный ресурс
            return CreatedAtAction(nameof(Get), new { id = aboutprojectsection.Id }, aboutprojectsection);
        }

        // PUT: api/Aboutprojectsections/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Aboutprojectsection aboutprojectsection)
        {
            if (id != aboutprojectsection.Id)
                return BadRequest();

            _context.Entry(aboutprojectsection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AboutprojectsectionExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent(); // 204
        }

        // DELETE: api/Aboutprojectsections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Aboutprojectsections.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.Aboutprojectsections.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AboutprojectsectionExists(int id)
        {
            return _context.Aboutprojectsections.Any(e => e.Id == id);
        }
    }
}
