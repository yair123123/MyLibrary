using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MyLibrary.Data;
using MyLibrary.Models;

namespace MyLibrary.Controllers
{
    public class ShelvesController : Controller
    {
        private readonly MyLibraryContext _context;

        public ShelvesController(MyLibraryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string category)
        {
            var categories = await _context.Shelf.Select(s => s.Category).Distinct().ToListAsync();
            var shelves = string.IsNullOrEmpty(category) ? await _context.Shelf.ToListAsync() : await _context.Shelf.Where(s => s.Category == category).ToListAsync();

            ViewBag.Categories = new SelectList(categories);
            ViewBag.SelectedCategory = category;

            return View(shelves);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelf = await _context.Shelf
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shelf == null)
            {
                return NotFound();
            }

            return View(shelf);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Category,Height,Width")] Shelf shelf)
        {
            ModelState.Remove("library");
            ModelState.Remove("mode");
            if (ModelState.IsValid)
            {
                if (_context.Library.Any(b => b.Category == shelf.Category))
                {
                    var libraryis = await _context.Library
                                               .FirstOrDefaultAsync(l => l.Category == shelf.Category);
                    shelf.LibraryId = libraryis.Id;
                    libraryis.CountShelves += 1;
                    shelf.rest = shelf.Width;
                    _context.Add(shelf);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(nameof(shelf.Category), "Category not found in the library");
                    return View(shelf);
                }
            }
            return View(shelf);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelf = await _context.Shelf.FindAsync(id);
            if (shelf == null)
            {
                return NotFound();
            }
            return View(shelf);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdLibrary,Height,Width")] Shelf shelf)
        {
            if (id != shelf.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shelf);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShelfExists(shelf.Id))
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
            return View(shelf);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelf = await _context.Shelf
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shelf == null)
            {
                return NotFound();
            }
            if (shelf.CountBooks == 0)
            {
                var name = shelf.Category;
                var library = await _context.Library
               .FirstOrDefaultAsync(l => l.Category == name);

                _context.Shelf.Remove(shelf);

                library.CountShelves -= 1;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(shelf);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shelf = await _context.Shelf.FindAsync(id);
            var name = shelf.Category;
            var library = await _context.Library
                           .FirstOrDefaultAsync(l => l.Category == name);
            if (shelf != null)
            {
                _context.Shelf.Remove(shelf);
                library.CountShelves -= 1;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShelfExists(int id)
        {
            return _context.Shelf.Any(e => e.Id == id);
        }
    }
}
