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
    public class TechniciansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TechniciansController(ApplicationDbContext context)
        {
            _context = context;
        }

    }
}
