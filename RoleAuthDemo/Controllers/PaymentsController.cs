using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoleAuthDemo.Data;
using RoleAuthDemo.Models;
using RoleAuthDemo.ViewModels;

namespace RoleAuthDemo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Payments
        public async Task<IActionResult> Index()
        {
            var payments = await _context.Payments
                .Include(p => p.Member)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(payments);
        }

        // GET: /Payments/Create
        public async Task<IActionResult> Create()
        {
            var members = await _context.Members
                .OrderBy(m => m.FirstName)
                .ThenBy(m => m.LastName)
                .ToListAsync();

            ViewBag.Members = new SelectList(members, "Id", "FirstName");
            return View();
        }

        // POST: /Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var members = await _context.Members
                    .OrderBy(m => m.FirstName)
                    .ThenBy(m => m.LastName)
                    .ToListAsync();

                ViewBag.Members = new SelectList(members, "Id", "FirstName", model.MemberId);
                return View(model);
            }

            var payment = new Payment
            {
                MemberId = model.MemberId,
                Amount = model.Amount,
                DueDate = model.DueDate.Date,
                Description = model.Description,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}