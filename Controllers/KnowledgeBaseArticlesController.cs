using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Data;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Controllers
{
    public class KnowledgeBaseArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KnowledgeBaseArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: KnowledgeBaseArticles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.KnowledgeBaseArticle.Include(k => k.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: KnowledgeBaseArticles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.KnowledgeBaseArticle == null)
            {
                return NotFound();
            }

            var knowledgeBaseArticle = await _context.KnowledgeBaseArticle
                .Include(k => k.Category)
                .FirstOrDefaultAsync(m => m.ArticleID == id);
            if (knowledgeBaseArticle == null)
            {
                return NotFound();
            }

            return View(knowledgeBaseArticle);
        }

        // GET: KnowledgeBaseArticles/Create
        public IActionResult Create()
        {
            ViewData["TicketCategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName");
            return View();
        }

        // POST: KnowledgeBaseArticles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArticleID,Title,Content,TicketCategoryID")] KnowledgeBaseArticle knowledgeBaseArticle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(knowledgeBaseArticle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TicketCategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName", knowledgeBaseArticle.TicketCategoryID);
            return View(knowledgeBaseArticle);
        }

        // GET: KnowledgeBaseArticles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.KnowledgeBaseArticle == null)
            {
                return NotFound();
            }

            var knowledgeBaseArticle = await _context.KnowledgeBaseArticle.FindAsync(id);
            if (knowledgeBaseArticle == null)
            {
                return NotFound();
            }
            ViewData["TicketCategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName", knowledgeBaseArticle.TicketCategoryID);
            return View(knowledgeBaseArticle);
        }

        // POST: KnowledgeBaseArticles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleID,Title,Content,TicketCategoryID")] KnowledgeBaseArticle knowledgeBaseArticle)
        {
            if (id != knowledgeBaseArticle.ArticleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(knowledgeBaseArticle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KnowledgeBaseArticleExists(knowledgeBaseArticle.ArticleID))
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
            ViewData["TicketCategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName", knowledgeBaseArticle.TicketCategoryID);
            return View(knowledgeBaseArticle);
        }

        // GET: KnowledgeBaseArticles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.KnowledgeBaseArticle == null)
            {
                return NotFound();
            }

            var knowledgeBaseArticle = await _context.KnowledgeBaseArticle
                .Include(k => k.Category)
                .FirstOrDefaultAsync(m => m.ArticleID == id);
            if (knowledgeBaseArticle == null)
            {
                return NotFound();
            }

            return View(knowledgeBaseArticle);
        }

        // POST: KnowledgeBaseArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.KnowledgeBaseArticle == null)
            {
                return Problem("Entity set 'ApplicationDbContext.KnowledgeBaseArticle'  is null.");
            }
            var knowledgeBaseArticle = await _context.KnowledgeBaseArticle.FindAsync(id);
            if (knowledgeBaseArticle != null)
            {
                _context.KnowledgeBaseArticle.Remove(knowledgeBaseArticle);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KnowledgeBaseArticleExists(int id)
        {
          return (_context.KnowledgeBaseArticle?.Any(e => e.ArticleID == id)).GetValueOrDefault();
        }
    }
}
