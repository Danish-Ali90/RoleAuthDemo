using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoleAuthDemo.Data;
using RoleAuthDemo.Models;

namespace RoleAuthDemo.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Member)
                .Include(b => b.Plot)
                .ToListAsync();

            return View(bookings);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            // Only show a available plots
            ViewBag.Plots = _context.Plots.Where(p => p.Status == true).ToList();
            ViewBag.Members = _context.Members.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                // 👇 Debug check
                Console.WriteLine($"MemberId: {booking.MemberId}, PlotId: {booking.PlotId}");

                _context.Bookings.Add(booking);

                var plot = await _context.Plots.FindAsync(booking.PlotId);
                if (plot != null)
                {
                    plot.Status = false;
                    _context.Plots.Update(plot);
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Booking created successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns if validation fails
            ViewBag.Plots = _context.Plots.Where(p => p.Status == true).ToList();
            ViewBag.Members = _context.Members.ToList();
            return View(booking);
        }
        // ✅ GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Member)
                .Include(b => b.Plot)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // ✅ POST: Bookings/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Plot)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking != null)
            {
                // 🔹 Make the plot active again
                if (booking.Plot != null)
                {
                    booking.Plot.Status = true;
                    _context.Plots.Update(booking.Plot);
                }

                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Booking deleted successfully and plot is now available!";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
