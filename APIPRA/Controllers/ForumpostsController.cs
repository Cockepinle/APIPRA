using System;
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
    public class ForumpostsController : Controller
    {
        private readonly PostgresContext _context;

        public ForumpostsController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Forumposts
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Forumposts.ToListAsync());
        }

        // GET: api/Forumposts/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var forumpost = await _context.Forumposts.FirstOrDefaultAsync(m => m.Id == id);
            if (forumpost == null)
                return NotFound();

            return View(forumpost);
        }

        // GET: api/Forumposts/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: api/Forumposts/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Title,Content,CreatedAt")] Forumpost forumpost)
        {
            if (ModelState.IsValid)
            {
                _context.Add(forumpost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(forumpost);
        }

        // GET: api/Forumposts/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var forumpost = await _context.Forumposts.FindAsync(id);
            if (forumpost == null)
                return NotFound();

            return View(forumpost);
        }

        // POST: api/Forumposts/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Title,Content,CreatedAt")] Forumpost forumpost)
        {
            if (id != forumpost.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(forumpost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ForumpostExists(forumpost.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(forumpost);
        }

        // GET: api/Forumposts/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var forumpost = await _context.Forumposts.FirstOrDefaultAsync(m => m.Id == id);
            if (forumpost == null)
                return NotFound();

            return View(forumpost);
        }

        // POST: api/Forumposts/Delete/5
        [HttpPost("Delete/{id}"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var forumpost = await _context.Forumposts.FindAsync(id);
            if (forumpost != null)
                _context.Forumposts.Remove(forumpost);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ForumpostExists(int id)
        {
            return _context.Forumposts.Any(e => e.Id == id);
        }
    }
}
