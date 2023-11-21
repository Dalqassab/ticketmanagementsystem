using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Data;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Messages
        public async Task<IActionResult> Index()
        {
            // Get the current user's ID
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Filter messages where the current user is either the sender or the receiver
            var applicationDbContext = _context.Message
                                               .Include(m => m.Receiver)
                                               .Include(m => m.Sender)
                                               .Where(m => m.SenderId == currentUserId || m.ReceiverID == currentUserId);

            return View(await applicationDbContext.ToListAsync());
          
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Message == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.Receiver)
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Messages/Create
        public IActionResult Create()
        {
            // Fetch users and project them into a new list with IDs and Usernames
            var users = _context.Users.Select(u => new { u.Id, u.UserName }).ToList();
            ViewData["ReceiverID"] = new SelectList(users, "UserName", "UserName");
            ViewData["SenderId"] = new SelectList(users, "UserName", "UserName");

            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Subject,Body,Timestampsend,SenderId,ReceiverID")] Message message)
        {
            message.Timestampsend = DateTime.Now;
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            message.Sender = await _context.Users.FindAsync(currentUserId);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == message.ReceiverID);
            message.Receiver =user;
            if (!ModelState.IsValid)
            {
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Fetch users and project them into a new list with IDs and Usernames
            var users = _context.Users.Select(u => new { u.Id, u.UserName }).ToList();
            ViewData["ReceiverID"] = new SelectList(users, "UserName", "UserName", message.ReceiverID);
            ViewData["SenderId"] = new SelectList(users, "UserName", "UserName", message.SenderId);
            return View(message);
        }


        // GET: Messages/Edit/5
        public async Task<IActionResult> Reply(int? id)
        {
            if (id == null || _context.Message == null)
            {
                return NotFound();
            }

            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            var users = _context.Users.Select(u => new { u.Id, u.UserName }).ToList();
           

            return View(message);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(int id,[Bind("Id,Subject,Body,Timestampsend,SenderId,ReceiverID")] Message message)
        {
            // Print a message indicating that the method has been accessed
            Console.WriteLine("Reply action method accessed!");
            message.Id = 0;
            message.Timestampsend = DateTime.Now;
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            message.Sender = await _context.Users.FindAsync(currentUserId);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == message.ReceiverID);
            message.Receiver = user;
            message.ParentMessage = await _context.Message.FindAsync(id); ;
            if (ModelState.IsValid)
            {
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Fetch users and project them into a new list with IDs and Usernames
            var users = _context.Users.Select(u => new { u.Id, u.UserName }).ToList();
            ViewData["ReceiverID"] = new SelectList(users, "UserName", "UserName");
            ViewData["SenderId"] = new SelectList(users, "UserName", "UserName");
            return View(message);
        }



        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Message == null)
            {
                return NotFound();
            }

            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            var users = _context.Users.Select(u => new { u.Id, u.UserName }).ToList();
            ViewData["ReceiverID"] = new SelectList(users, "UserName", "UserName");
            ViewData["SenderId"] = new SelectList(users, "UserName", "UserName");
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Subject,Body,Timestampsend,SenderId,ReceiverID")] Message message)
        {
            if (id != message.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.Id))
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
            var users = _context.Users.Select(u => new { u.Id, u.UserName }).ToList();
            ViewData["ReceiverID"] = new SelectList(users, "UserName", "UserName");
            ViewData["SenderId"] = new SelectList(users, "UserName", "UserName");
            return View(message);
        }

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Message == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.Receiver)
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Message == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Message'  is null.");
            }
            var message = await _context.Message.FindAsync(id);
            if (message != null)
            {
                _context.Message.Remove(message);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
          return (_context.Message?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
