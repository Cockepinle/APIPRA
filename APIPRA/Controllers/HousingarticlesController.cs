using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIPRA.Models;

namespace APIPRA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HousingarticlesController : Controller
    {
        private readonly PostgresContext _context;

        public HousingarticlesController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Housingarticles
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Housingarticles.ToListAsync());
        }

        // GET: api/Housingarticles/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var housingarticle = await _context.Housingarticles.FirstOrDefaultAsync(m => m.Id == id);
            if (housingarticle == null)
                return NotFound();

            return View(housingarticle);
        }

        // GET: api/Housingarticles/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: api/Housingarticles/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,TypeId")] Housingarticle housingarticle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(housingarticle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(housingarticle);
        }

        // GET: api/Housingarticles/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var housingarticle = await _context.Housingarticles.FindAsync(id);
            if (housingarticle == null)
                return NotFound();

            return View(housingarticle);
        }

        // POST: api/Housingarticles/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,TypeId")] Housingarticle housingarticle)
        {
            if (id != housingarticle.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(housingarticle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HousingarticleExists(housingarticle.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(housingarticle);
        }

        // GET: api/Housingarticles/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var housingarticle = await _context.Housingarticles.FirstOrDefaultAsync(m => m.Id == id);
            if (housingarticle == null)
                return NotFound();

            return View(housingarticle);
        }

        // POST: api/Housingarticles/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var housingarticle = await _context.Housingarticles.FindAsync(id);
            if (housingarticle != null)
                _context.Housingarticles.Remove(housingarticle);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HousingarticleExists(int id)
        {
            return _context.Housingarticles.Any(e => e.Id == id);
        }
    }
}
