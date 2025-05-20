using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIPRA.Models;

namespace APIPRA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HousingdocumentsController : Controller
    {
        private readonly PostgresContext _context;

        public HousingdocumentsController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Housingdocuments
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Housingdocuments.ToListAsync());
        }

        // GET: api/Housingdocuments/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var housingdocument = await _context.Housingdocuments.FirstOrDefaultAsync(m => m.Id == id);
            if (housingdocument == null)
                return NotFound();

            return View(housingdocument);
        }

        // GET: api/Housingdocuments/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: api/Housingdocuments/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,FileUrl")] Housingdocument housingdocument)
        {
            if (ModelState.IsValid)
            {
                _context.Add(housingdocument);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(housingdocument);
        }

        // GET: api/Housingdocuments/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var housingdocument = await _context.Housingdocuments.FindAsync(id);
            if (housingdocument == null)
                return NotFound();

            return View(housingdocument);
        }

        // POST: api/Housingdocuments/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,FileUrl")] Housingdocument housingdocument)
        {
            if (id != housingdocument.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(housingdocument);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HousingdocumentExists(housingdocument.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(housingdocument);
        }

        // GET: api/Housingdocuments/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var housingdocument = await _context.Housingdocuments.FirstOrDefaultAsync(m => m.Id == id);
            if (housingdocument == null)
                return NotFound();

            return View(housingdocument);
        }

        // POST: api/Housingdocuments/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var housingdocument = await _context.Housingdocuments.FindAsync(id);
            if (housingdocument != null)
            {
                _context.Housingdocuments.Remove(housingdocument);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool HousingdocumentExists(int id)
        {
            return _context.Housingdocuments.Any(e => e.Id == id);
        }
    }
}
