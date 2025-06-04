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
        public async Task<ActionResult<IEnumerable<TestimageReadDto>>> GetTestimages()
        {
            var testimages = await _context.Testimages.ToListAsync();

            var result = testimages.Select(t => new TestimageReadDto
            {
                Id = t.Id,
                CreatedAt = t.CreatedAt,
                Description = t.Description,
                ImageUrl = t.ImageUrl,
                TestId = t.TestId
            });

            return Ok(result);
        }

        // GET: api/Testimages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestimageReadDto>> GetTestimage(int id)
        {
            var testimage = await _context.Testimages.FindAsync(id);

            if (testimage == null)
            {
                return NotFound();
            }

            var result = new TestimageReadDto
            {
                Id = testimage.Id,
                CreatedAt = testimage.CreatedAt,
                Description = testimage.Description,
                ImageUrl = testimage.ImageUrl,
                TestId = testimage.TestId
            };

            return Ok(result);
        }

        // POST: api/Testimages
        [HttpPost]
        public async Task<ActionResult<TestimageReadDto>> PostTestimage(TestimageCreateDto dto)
        {
            var testimage = new Testimage
            {
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                TestId = dto.TestId
            };

            _context.Testimages.Add(testimage);
            await _context.SaveChangesAsync();

            var result = new TestimageReadDto
            {
                Id = testimage.Id,
                CreatedAt = testimage.CreatedAt,
                Description = testimage.Description,
                ImageUrl = testimage.ImageUrl,
                TestId = testimage.TestId
            };

            return CreatedAtAction(nameof(GetTestimage), new { id = testimage.Id }, result);
        }

        // PUT: api/Testimages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTestimage(int id, TestimageCreateDto dto)
        {
            var testimage = await _context.Testimages.FindAsync(id);

            if (testimage == null)
            {
                return NotFound();
            }

            testimage.Description = dto.Description;
            testimage.ImageUrl = dto.ImageUrl;
            testimage.TestId = dto.TestId;

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

    // DTO классы можно вынести в отдельный файл, например, TestimageDto.cs
    public class TestimageCreateDto
    {
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int TestId { get; set; }
    }

    public class TestimageReadDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int TestId { get; set; }
    }
}
