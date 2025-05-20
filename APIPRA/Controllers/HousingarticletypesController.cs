using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIPRA.Models;

namespace APIPRA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HousingarticletypesController : Controller
    {
        private readonly PostgresContext _context;

        public HousingarticletypesController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Housingarticletypes
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Housingarticletypes.ToListAsync());
        }

        // GET: api/Housingarticletypes/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var housingarticletype = await _context.Housingarticletypes.FirstOrDefaultAsync(m => m.Id == id);
            if (housingarticletype == null)
                return NotFound();

            return View(housingarticletype);
        }

        // GET: api/Housingarticletypes/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: api/Housingarticletypes/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Housingarticletype housingarticletype)
        {
            if (ModelState.IsValid)
            {
                _context.Add(housingarticletype);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(housingarticletype);
        }

        // GET: api/Housingarticletypes/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var housingarticletype = await _context.Housingarticletypes.FindAsync(id);
            if (housingarticletype == null)
                return NotFound();

            return View(housingarticletype);
        }

        // POST: api/Housingarticletypes/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Housingarticletype housingarticletype)
        {
            if (id != housingarticletype.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(housingarticletype);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HousingarticletypeExists(housingarticletype.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(housingarticletype);
        }

        // GET: api/Housingarticletypes/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var housingarticletype = await _context.Housingarticletypes.FirstOrDefaultAsync(m => m.Id == id);
            if (housingarticletype == null)
                return NotFound();

            return View(housingarticletype);
        }

        // POST: api/Housingarticletypes/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var housingarticletype = await _context.Housingarticletypes.FindAsync(id);
            if (housingarticletype != null)
            {
                _context.Housingarticletypes.Remove(housingarticletype);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HousingarticletypeExists(int id)
        {
            return _context.Housingarticletypes.Any(e => e.Id == id);
        }
    }
}
