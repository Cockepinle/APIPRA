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
    public class ForumrepliesController : Controller
    {
        private readonly PostgresContext _context;

        public ForumrepliesController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Forumreplies
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Forumreplies.ToListAsync());
        }

        // GET: api/Forumreplies/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumreply = await _context.Forumreplies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (forumreply == null)
            {
                return NotFound();
            }

            return View(forumreply);
        }

        // GET: api/Forumreplies/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: api/Forumreplies/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PostId,UserId,Content,CreatedAt")] Forumreply forumreply)
        {
            if (ModelState.IsValid)
            {
                _context.Add(forumreply);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(forumreply);
        }

        // GET: api/Forumreplies/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumreply = await _context.Forumreplies.FindAsync(id);
            if (forumreply == null)
            {
                return NotFound();
            }
            return View(forumreply);
        }

        // POST: api/Forumreplies/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PostId,UserId,Content,CreatedAt")] Forumreply forumreply)
        {
            if (id != forumreply.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(forumreply);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ForumreplyExists(forumreply.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(forumreply);
        }

        // GET: api/Forumreplies/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumreply = await _context.Forumreplies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (forumreply == null)
            {
                return NotFound();
            }

            return View(forumreply);
        }

        // POST: api/Forumreplies/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var forumreply = await _context.Forumreplies.FindAsync(id);
            if (forumreply != null)
            {
                _context.Forumreplies.Remove(forumreply);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ForumreplyExists(int id)
        {
            return _context.Forumreplies.Any(e => e.Id == id);
        }
    }
}
