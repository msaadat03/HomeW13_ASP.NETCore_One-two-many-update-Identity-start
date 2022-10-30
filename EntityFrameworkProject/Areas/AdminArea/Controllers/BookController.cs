using EntityFrameworkProject.Data;
using EntityFrameworkProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class BookController : Controller
    {
        private readonly AppDbContext _context;
        public BookController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var book = await _context.Books.Include(m => m.BookTags).ThenInclude(m => m.Book).ToListAsync();
            return View(book);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Tags = _context.Tags.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            CheckTags(book);

            if (!ModelState.IsValid)
            {
                ViewBag.Tags = _context.Tags.ToListAsync();
            }

            if (book.TagIds != null)
            {
                foreach (var tagId in book.TagIds)
                {
                    BookTag bookTag = new BookTag
                    {
                        TagId = tagId
                    };
                }
            }

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Book existBook = await _context.Books.Include(m => m.BookTags).FirstOrDefaultAsync(m => m.Id == id);

            if (existBook == null)
            {
                return RedirectToAction("Dashboard");
            }

            ViewBag.Tags = await _context.Tags.ToListAsync();

            existBook.TagIds = existBook.BookTags.Select(m => m.TagId).ToList();

            return View(existBook);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book book)
        {
            Book existBook = await _context.Books.Include(m => m.BookTags).FirstOrDefaultAsync(m => m.Id == book.Id);

            if (existBook == null)
            {
                return RedirectToAction("Error", "Dashboard");
            }

            CheckTags(book);

            if (ModelState.IsValid)
            {
                ViewBag.Tags = await _context.Tags.ToListAsync();
                return View();
            }

            existBook.BookTags.RemoveAll(m => !book.TagIds.Contains(m.TagId));

            foreach (var tagId in book.TagIds.Where(m => !existBook.BookTags.Any(b => b.TagId == m)))
            {
                BookTag bookTag = new BookTag
                {
                    TagId = tagId
                };

                existBook.BookTags.Add(bookTag);
            }

            existBook.Name = book.Name;
            existBook.Description = book.Description;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private void CheckTags(Book book)
        {
            if (book.TagIds != null)
            {
                foreach (var tagId in book.TagIds)
                {
                    if (_context.Tags.Any(m => m.Id == tagId))
                    {
                        ModelState.AddModelError("TagIds", "Tag not found!");
                        return;
                    }
                }
            }
        }
    }
}
