using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Data;
using TicketManagementSystem.Models;
using static TicketManagementSystem.Models.Ticket;

namespace TicketManagementSystem.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public TicketsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var tickets = (IList<Ticket>)null; // Correct the type casting for tickets
            string userId = _userManager.GetUserId(User);
            if (User.IsInRole("Member"))
            {
                tickets = await _context.Ticket.Include(t => t.Category).Include(t => t.User).Include(t => t.Facilitator).DefaultIfEmpty().Where(t => t.Id == userId).ToListAsync();
            }
            else{
                tickets = await _context.Ticket.Include(t => t.Category).Include(t => t.User).Include(t => t.Facilitator).DefaultIfEmpty().ToListAsync();
            }

            if (tickets == null)
            {
                // Handle the null case, e.g., redirect to an error page or set a default value
                return RedirectToAction("Error"); // Or any other appropriate action
            }
            return View(tickets);
        }


        [Authorize]
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
        // GET: Tickets/Edit/<int>
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Tickets/Edit/{id}/{Ticket}")]
        public async Task<IActionResult> Edit(int id, [Bind("TicketID,Subject,Description,DateCreated,DateUpdated,Priority,Status,Id,TicketCategoryID")] Ticket ticket)
        {
            ticket.DateUpdated = DateTime.Now;
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


        [HttpPost]
        [Route("Tickets/UpdateStatus/{id}/{status}")]
        public async Task<IActionResult> UpdateStatus(int id, TicketStatus status)
        {
            // Retrieve the ticket data based on the provided ID
            var ticket = await _context.Ticket.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            // Update the ticket's status with the provided value
            ticket.Status = status;

            // Save the updated ticket data to the database
            _context.Update(ticket);
            await _context.SaveChangesAsync();

            // Return a successful response
            return RedirectToAction(nameof(Index)); ;
        }

        [HttpGet]
        [Route("Tickets/UpdateStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int? id)
        {
            // Retrieve the ticket data based on the provided ID
            var ticket = await _context.Ticket.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }


            // Return the view with the updated status view model
            return View(ticket);
        }

        [HttpGet]
        [Route("Tickets/Takeit/{id}")]
        public async Task<IActionResult> TakeIt(int? id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            return View(ticket);
        }



        [HttpPost]
        [Route("Tickets/TakeIt/{id}")]
        public async Task<IActionResult> TakeIt(int id)
        {
            // Retrieve the ticket data based on the provided ID
            var ticket = await _context.Ticket.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            // Update the ticket's status with the provided value

            ApplicationUser user = (ApplicationUser)await _userManager.FindByIdAsync(ticket.Id);

            ticket.TechnicianId = user.Id;


            // Save the updated ticket data to the database
            _context.Update(ticket);
            await _context.SaveChangesAsync();

            // Return a successful response
            return RedirectToAction(nameof(Index)); ;
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
