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
    [Authorize]
    public class TechniciansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TechniciansController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Technicians        
        public async Task<IActionResult> Index()
        {
            // Retrieve all technicians from the database
            var technicians = await _context.Technician.ToListAsync();

            // Create a dictionary to map technician IDs to usernames
            var technicianIdToUsername = new Dictionary<string, string>();
            foreach (var technician in technicians)
            {
                // Get the username for the current technician
                var username = await _userManager.FindByIdAsync(technician.TechnicianId);

                // Add the technician ID and username to the dictionary
                technicianIdToUsername[technician.TechnicianId] = username.UserName;
            }

            // Replace technician IDs with usernames in the technicians list
            foreach (var technician in technicians)
            {
                technician.TechnicianId = technicianIdToUsername[technician.TechnicianId];
            }

            // Return the modified technicians list to the view
            return View(technicians);
        }

        // GET: Technicians/GetName/5
        public async Task<string> GetUsernameById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return user.UserName;
            }
            else
            {
                return null;
            }
        }



        // GET: Technicians/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Technician == null)
            {
                return NotFound();
            }

            var technician = await _context.Technician
                .FirstOrDefaultAsync(m => m.TechnicianId == id);
            if (technician == null)
            {
                return NotFound();
            }

            return View(technician);
        }
        
        // GET: Technicians/Create
        public IActionResult Create()
        {
          
            return View();
        }
        [Authorize]
        // POST: Technicians/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TechnicianId,areaofexpertise,yearsofexperience")] Technician technician)
        {
            if (User.Identity.IsAuthenticated & !User.IsInRole("Technician"))
            {
                string userId = _userManager.GetUserId(User); ; 
                technician.TechnicianId = userId;

                if (!ModelState.IsValid)
                {
                    _context.Add(technician);
                    await _context.SaveChangesAsync();
                    var user = await _userManager.FindByIdAsync(technician.TechnicianId);
                    await _userManager.RemoveFromRoleAsync(user, "Member");
                    await _userManager.AddToRoleAsync(user, "Technician");
                    return RedirectToAction(nameof(Index));
                }
                return View(technician);
            }
            else
            {
                // Handle the case when no user is logged in
                return View();
            }


            
        }

        // GET: Technicians/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Technician == null)
            {
                return NotFound();
            }

            var technician = await _context.Technician.FindAsync(id);
            if (technician == null)
            {
                return NotFound();
            }
            return View(technician);
        }

        // POST: Technicians/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TechnicianId,areaofexpertise,yearsofexperience")] Technician technician)
        {
            if (id != technician.TechnicianId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(technician);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TechnicianExists(technician.TechnicianId))
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
            return View(technician);
        }

        // GET: Technicians/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Technician == null)
            {
                return NotFound();
            }

            var technician = await _context.Technician
                .FirstOrDefaultAsync(m => m.TechnicianId == id);
            if (technician == null)
            {
                return NotFound();
            }

            return View(technician);
        }

        // POST: Technicians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Technician == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Technician'  is null.");
            }
            var technician = await _context.Technician.FindAsync(id);
            if (technician != null)
            {
                _context.Technician.Remove(technician);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TechnicianExists(string id)
        {
          return (_context.Technician?.Any(e => e.TechnicianId == id)).GetValueOrDefault();
        }
    }
}
