using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Data;
using MyLibrary.Models;

namespace MyLibrary.Controllers
{
    public class BooksController : Controller
    {
        private readonly MyLibraryContext _context;

        public BooksController(MyLibraryContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string category)
        {
            var categories = await _context.Book.Select(s => s.Category).Distinct().ToListAsync();
            var books = string.IsNullOrEmpty(category) ? await _context.Book.ToListAsync() : await _context.Book.Where(s => s.Category == category).ToListAsync();

            ViewBag.Categories = new SelectList(categories);
            ViewBag.SelectedCategory = category;

            return View(books);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Height,Width,Category")] Book book)
        {
            ModelState.Remove("SetBook");
            ModelState.Remove("shelf");

            if (ModelState.IsValid)
            {
                if (!_context.Library.Any(b => b.Category == book.Category))
                {
                    ModelState.AddModelError(nameof(book.Category), "לא נמצאה ספרייה לקטגוריה זו ");
                    return View();
                }
                    var shelf = await _context.Shelf
                            .Where(s => s.Category == book.Category && s.rest - book.Width >= 0 && s.Height - book.Height >= 0)
                            .FirstOrDefaultAsync();

                if (shelf == null)
                {
                    ModelState.AddModelError("", "אין מספיק מקום יש ליצור מדף חדש");
                    return View(book);
                }
                int Rest = shelf.Height - book.Height;
                if (Rest > 10)
                {
                    ViewBag.WarningMessage = $" אם תוסיף את הספר לפה יישאר לך {Rest}ס''מ של רווח פנוי  ";
                    return View(book);
                }

                book.ShelfId = shelf.Id; 

                _context.Book.Add(book); 

                shelf.rest -= book.Width; 
                _context.Update(shelf); 

                await _context.SaveChangesAsync(); 

                return RedirectToAction(nameof(Index)); 
            }

            return View(book); 
        }


        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ShelfId,Height,Width,Category,setId")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
