using EntityFrameworkProject.Data;
using EntityFrameworkProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class BlogHeaderController : Controller
    {
        private readonly AppDbContext _context;

        public BlogHeaderController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            BlogHeader blogHeader = await _context.BlogHeaders.Where(m => !m.IsDeleted).AsNoTracking().FirstOrDefaultAsync();
            ViewBag.count = await _context.BlogHeaders.Where(m => !m.IsDeleted).CountAsync();
            return View(blogHeader);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogHeader blogHeader)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                await _context.BlogHeaders.AddAsync(blogHeader);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }

        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            BlogHeader blogHeader = await _context.BlogHeaders.FindAsync(id);
            if (blogHeader == null) return NotFound();
            return View(blogHeader);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                BlogHeader blogHeader = await _context.BlogHeaders.FirstOrDefaultAsync(m => m.Id == id);

                if (blogHeader is null) return NotFound();

                return View(blogHeader);

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogHeader blogHeader)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(blogHeader);
                }

                BlogHeader dbBlogHeader = await _context.BlogHeaders.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

                if (dbBlogHeader is null) return NotFound();

                if (dbBlogHeader.Title.ToLower().Trim() == blogHeader.Title.ToLower().Trim())
                {
                    return RedirectToAction(nameof(Index));
                }

                _context.BlogHeaders.Update(blogHeader);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            BlogHeader blogHeader = await _context.BlogHeaders.FirstOrDefaultAsync(m => m.Id == id);

            blogHeader.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
