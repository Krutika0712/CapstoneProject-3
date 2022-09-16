using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyRestaurant.Data;
using MyRestaurant.Models;

namespace MyRestaurant.Areas.User.Controllers
{
    [Area("User")]
    public class MenuItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MenuItemsController> _logger;
        public MenuItemsController(ApplicationDbContext context, ILogger<MenuItemsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Demo/MenuItems
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MenuItem.Include(m => m.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> GetMenuItemOfCategory(int filterCategoryId)
        {
            var viewmodel = await _context.MenuItem
                                          .Where(m => m.CategoryId == filterCategoryId)
                                          .Include(m => m.Category)
                                          .ToListAsync();

            return View(viewName: "Index", model: viewmodel);
        }
        // GET: Demo/MenuItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItem
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.MenuItemId == id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // GET: Demo/MenuItems/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Demo/MenuItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MenuItemId,MenuItemName,MenuPrice,IsEnabled,CategoryId")] MenuItem menuItem)
        {
            menuItem.MenuItemName = menuItem.MenuItemName.Trim();

            // Validation Checks - Server-side validation
            bool duplicateExists = _context.MenuItem 
              .Any(c => c.MenuItemName == menuItem.MenuItemName);
            if (duplicateExists)
            {
                ModelState.AddModelError("MenuItemName", "Duplicate Menu Found!");
            }

          
          
            if (ModelState.IsValid)
            {
                _context.Add(menuItem);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Created a New Category: ID = {menuItem.MenuItemId} !");
                return RedirectToAction(nameof(Index));

            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", menuItem.CategoryId);
            return View(menuItem);
        }

        // GET: Demo/MenuItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItem.FindAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", menuItem.CategoryId);
            return View(menuItem);
        }

        // POST: Demo/MenuItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MenuItemId,MenuItemName,MenuPrice,IsEnabled,CategoryId")] MenuItem menuItem)
        {
            if (id != menuItem.MenuItemId)
            {
                return NotFound();
            }

            menuItem.MenuItemName = menuItem.MenuItemName.Trim();

            // Validation Checks - Server-side validation
            bool duplicateExists = _context.MenuItem
                .Any(c => c.MenuItemName == menuItem.MenuItemName && c.MenuItemId != menuItem.MenuItemId);
            if (duplicateExists)
            {
                ModelState.AddModelError("MenuItemName", "Duplicate Menu Found!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuItemExists(menuItem.MenuItemId))
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
            
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", menuItem.CategoryId);
            return View(menuItem);
        }

        // GET: Demo/MenuItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItem
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.MenuItemId == id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // POST: Demo/MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuItem = await _context.MenuItem.FindAsync(id);
            _context.MenuItem.Remove(menuItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuItemExists(int id)
        {
            return _context.MenuItem.Any(e => e.MenuItemId == id);
        }
    }
}
