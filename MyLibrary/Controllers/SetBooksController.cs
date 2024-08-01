using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Data;
using MyLibrary.Models;

namespace MyLibrary.Controllers
{
    public class SetBooks : Controller
    {
        private readonly MyLibraryContext _context;

        public SetBooks(MyLibraryContext context)
        {
            _context = context;
        }

        // GET: Books
        public IActionResult Index(string category)
        {
            var categories = _context.SetBook.Select(s => s.Category).Distinct().ToList();
            var books = string.IsNullOrEmpty(category)
                ? _context.SetBook.ToList()
                : _context.SetBook.Where(s => s.Category == category).ToList();

            ViewBag.Categories = new SelectList(categories);
            ViewBag.SelectedCategory = category;

            return View(books);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _context.SetBook.FirstOrDefault(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Height,Width,Category")] SetBook book, int isConfirmed, int choiceShelf)
        {
            ModelState.Remove("SetBook");
            ModelState.Remove("shelf");

            if (!ModelState.IsValid)
            {
                return View(book);
            }

            var shelves = GetShelvesByCategory(book.Category);

            if (isConfirmed == 1)
            {
                return HandleInitialConfirmation(book, shelves);
            }

            if (isConfirmed == 2)
            {
                return HandleFinalConfirmation(book, shelves, choiceShelf);
            }
            if (isConfirmed == 3)
            {
                var chosenShelf = shelves.FirstOrDefault(s => s.Id == choiceShelf);
                return HandleFinalConfirmationEnd(book, chosenShelf);
            }

            return View(book);
        }

        private IActionResult HandleInitialConfirmation(SetBook book, List<Shelf> shelves)
        {
            if (!shelves.Any())
            {
                ModelState.AddModelError(nameof(book.Category), "לא נמצאה ספרייה לקטגוריה זו");
                return View(book);
            }

            foreach (var shelf in shelves)
            {
                shelf.Mode = GetShelfMode(shelf, book.Height, book.Width);
            }

            ViewBag.Shelves = shelves;
            ViewBag.FormType = true;
            return View();
        }

        private IActionResult HandleFinalConfirmation(SetBook book, List<Shelf> shelves, int choiceId)
        {
            var chosenShelf = shelves.FirstOrDefault(s => s.Id == choiceId);
            if (chosenShelf == null || !HasSufficientSpace(chosenShelf, book))
            {
                ModelState.AddModelError("", "אין מספיק מקום יש לבחור מדף אחר");
                return View(book);
            }
            int space = chosenShelf.Height - book.Height;
            if (space > 10)
            {
                ViewBag.WarningMessage = $"לא מומלץ יישאר לך {space} ס''מ מיותרים";
                ViewBag.chosenShelf = choiceId;
                return View(book);
            }
            book.ShelfId = chosenShelf.Id;
            chosenShelf.CountBooks++;
            chosenShelf.rest -= book.Width;

            _context.Update(chosenShelf);
            _context.SetBook.Add(book);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private IActionResult HandleFinalConfirmationEnd(SetBook book, Shelf chosenShelf)
        {
            book.ShelfId = chosenShelf.Id;
            chosenShelf.CountBooks++;
            chosenShelf.rest -= book.Width;

            _context.Update(chosenShelf);
            _context.SetBook.Add(book);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private List<Shelf> GetShelvesByCategory(string category)
        {
            return _context.Shelf.Where(s => s.Category == category).ToList();
        }

        private string GetShelfMode(Shelf shelf, int bookHeight, int bookWidth)
        {
            int restHeight = shelf.Height - bookHeight;
            int restWidth = shelf.rest - bookWidth;
            if (restWidth < 0) { return "לא ניתן לשייך למדף זה (חוסר מקום)"; }
            return restHeight switch
            {
                > 10 => "לא מומלץ (רווח גובה יותר מ10 ס''מ)",
                < 0 => "לא ניתן לשייך למדף זה (חוסר מקום)",
                _ => "מומלץ"
            };
        }

        private bool HasSufficientSpace(Shelf shelf, SetBook book)
        {
            return shelf.rest >= book.Width && shelf.Height >= book.Height;
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _context.Book.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,ShelfId,Height,Width,Category,setId")] Book book)
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
                    _context.SaveChanges();
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
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _context.Book.FirstOrDefault(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _context.Book.Find(id);
            var name = book.Category;
            var shelf = _context.Shelf.FirstOrDefault(l => l.Category == name);
            if (book != null)
            {
                shelf.rest += book.Width;
                shelf.CountBooks -= 1;
                _context.Book.Remove(book);
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
