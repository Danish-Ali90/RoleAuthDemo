using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoleAuthDemo.Data;
using RoleAuthDemo.Models;

namespace RoleAuthDemo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MembersController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            var members = await _context.Members
                .OrderBy(m => m.FirstName)
                .ToListAsync();

            return View(members);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member, IFormFile? ProfileImage)
        {
            if (ModelState.IsValid)
            {
                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/members");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfileImage.CopyToAsync(fileStream);
                    }

                    member.ProfileImagePath = "/uploads/members/" + uniqueFileName;
                }

                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var member = await _context.Members.FindAsync(id);
            if (member == null) return NotFound();

            return View(member);
        }

        // POST: Members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Member member, IFormFile? ProfileImage)
        {
            if (id != member.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingMember = await _context.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                    if (existingMember == null) return NotFound();

                    // ✅ Handle new profile picture upload
                    if (ProfileImage != null && ProfileImage.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/members");
                        Directory.CreateDirectory(uploadsFolder);

                        string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // ✅ Delete old image if exists
                        if (!string.IsNullOrEmpty(existingMember.ProfileImagePath))
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingMember.ProfileImagePath.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ProfileImage.CopyToAsync(fileStream);
                        }

                        member.ProfileImagePath = "/uploads/members/" + uniqueFileName;
                    }
                    else
                    {
                        // Keep existing image if not replaced
                        member.ProfileImagePath = existingMember.ProfileImagePath;
                    }

                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var member = await _context.Members.FirstOrDefaultAsync(m => m.Id == id);
            if (member == null) return NotFound();

            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var member = await _context.Members.FirstOrDefaultAsync(m => m.Id == id);
            if (member == null) return NotFound();

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                // ✅ Delete stored image
                if (!string.IsNullOrEmpty(member.ProfileImagePath))
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, member.ProfileImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Members.Remove(member);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
    }
}
