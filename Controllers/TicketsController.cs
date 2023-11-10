using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Data;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public TicketsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
     
        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Ticket.Include(t => t.Category).Include(t => t.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.Category)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TicketID == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }
        [Authorize]
        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["TicketCategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName");
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }
        [Authorize]
        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketID,Subject,Description,DateCreated,DateUpdated,Priority,Status,Id,TicketCategoryID")] Ticket ticket)
        {
            if (ticket.Id != null)
            {
                // Use UserManager to find the user by their ID
                ApplicationUser user = (ApplicationUser)await _userManager.FindByIdAsync(ticket.Id);

                if (user != null)
                {
                    // Associate the retrieved user with the ticket
                    ticket.User = user;
                }
                else
                {
                    // User with the provided ID is not found
                    // Handle this situation by adding an error to the ModelState
                    ModelState.AddModelError("Id", "The user associated with the provided ID was not found.");
                }
            }
            else
            {
                // No ID was provided for the ticket
                ModelState.AddModelError("Id", "No user ID was provided for the ticket.");
            }
            ticket.DateCreated = DateTime.Now;
            ticket.DateUpdated = DateTime.Now;
            var userId = _userManager.GetUserId(User); // Gets the ID of the currently logged-in user
            if (!ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TicketCategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName", ticket.TicketCategoryID);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", ticket.Id);
            return View(ticket);
        }
        [Authorize]
        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["TicketCategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName", ticket.TicketCategoryID);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", ticket.Id);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketID,Subject,Description,DateCreated,DateUpdated,Priority,Status,Id,TicketCategoryID")] Ticket ticket)
        {
            if (id != ticket.TicketID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.TicketID))
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
            ViewData["TicketCategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName", ticket.TicketCategoryID);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", ticket.Id);
            return View(ticket);
        }
        [Authorize]
        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.Category)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TicketID == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }
        [Authorize]
        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ticket == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Ticket'  is null.");
            }
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket != null)
            {
                _context.Ticket.Remove(ticket);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
          return (_context.Ticket?.Any(e => e.TicketID == id)).GetValueOrDefault();
        }
    }
}
