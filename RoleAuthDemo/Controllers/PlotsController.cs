using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoleAuthDemo.Data;
using RoleAuthDemo.Models;

namespace RoleAuthDemo.Controllers
{
    [Authorize(Roles = "Admin")] // ✅ Only Admin can manage Plots
    public class PlotsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlotsController> _logger;

        public PlotsController(ApplicationDbContext context, ILogger<PlotsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ✅ GET: Plots
        public async Task<IActionResult> Index()
        {
            var plots = await _context.Plots
                .OrderBy(p => p.PlotNumber)
                .ToListAsync();
            return View(plots);
        }

        // ✅ GET: Plots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var plot = await _context.Plots.FirstOrDefaultAsync(p => p.Id == id);
            if (plot == null) return NotFound();

            return View(plot);
        }

        // ✅ GET: Plots/Create
        public IActionResult Create()
        {
            return View();
        }

        // ✅ POST: Plots/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Plot plot)
        {
            if (!string.IsNullOrWhiteSpace(plot.PlotNumber))
                plot.PlotNumber = plot.PlotNumber.Trim();

            // Check for duplicate PlotNumber
            if (await _context.Plots.AnyAsync(p => p.PlotNumber == plot.PlotNumber))
                ModelState.AddModelError("PlotNumber", "This Plot Number already exists.");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(plot);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "✅ Plot created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while creating plot");
                    ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                }
            }

            return View(plot);
        }

        // ✅ GET: Plots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var plot = await _context.Plots.FindAsync(id);
            if (plot == null) return NotFound();

            return View(plot);
        }

        // ✅ POST: Plots/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Plot plot)
        {
            if (id != plot.Id) return NotFound();

            if (!string.IsNullOrWhiteSpace(plot.PlotNumber))
                plot.PlotNumber = plot.PlotNumber.Trim();

            // Unique PlotNumber check (excluding self)
            if (await _context.Plots.AnyAsync(p => p.PlotNumber == plot.PlotNumber && p.Id != plot.Id))
                ModelState.AddModelError("PlotNumber", "This Plot Number already exists.");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plot);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "✅ Plot updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlotExists(plot.Id))
                        return NotFound();
                    else
                        throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while editing plot");
                    ModelState.AddModelError("", "An error occurred while updating. Please try again.");
                }
            }

            return View(plot);
        }

        // ✅ GET: Plots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var plot = await _context.Plots.FirstOrDefaultAsync(p => p.Id == id);
            if (plot == null) return NotFound();

            return View(plot);
        }

        // ✅ POST: Plots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var plot = await _context.Plots.FindAsync(id);
                if (plot != null)
                {
                    _context.Plots.Remove(plot);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "🗑️ Plot deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Plot not found.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting plot");
                TempData["Error"] = "An error occurred while deleting.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PlotExists(int id)
        {
            return _context.Plots.Any(e => e.Id == id);
        }
    }
}
