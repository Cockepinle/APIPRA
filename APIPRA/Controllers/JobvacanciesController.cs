using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIPRA.Models;

namespace APIPRA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobvacanciesController : Controller
    {
        private readonly PostgresContext _context;

        public JobvacanciesController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Jobvacancies
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Jobvacancies.ToListAsync());
        }

        // GET: api/Jobvacancies/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var jobvacancy = await _context.Jobvacancies.FirstOrDefaultAsync(m => m.Id == id);
            if (jobvacancy == null)
                return NotFound();

            return View(jobvacancy);
        }

        // GET: api/Jobvacancies/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: api/Jobvacancies/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,EmployerName,Location,ContactInfo,CreatedAt")] Jobvacancy jobvacancy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobvacancy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobvacancy);
        }

        // GET: api/Jobvacancies/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var jobvacancy = await _context.Jobvacancies.FindAsync(id);
            if (jobvacancy == null)
                return NotFound();

            return View(jobvacancy);
        }

        // POST: api/Jobvacancies/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,EmployerName,Location,ContactInfo,CreatedAt")] Jobvacancy jobvacancy)
        {
            if (id != jobvacancy.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobvacancy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobvacancyExists(jobvacancy.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(jobvacancy);
        }

        // GET: api/Jobvacancies/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var jobvacancy = await _context.Jobvacancies.FirstOrDefaultAsync(m => m.Id == id);
            if (jobvacancy == null)
                return NotFound();

            return View(jobvacancy);
        }

        // POST: api/Jobvacancies/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobvacancy = await _context.Jobvacancies.FindAsync(id);
            if (jobvacancy != null)
            {
                _context.Jobvacancies.Remove(jobvacancy);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool JobvacancyExists(int id)
        {
            return _context.Jobvacancies.Any(e => e.Id == id);
        }
    }
}
