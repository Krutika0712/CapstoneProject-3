using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyRestaurant.Data;
using MyRestaurant.Models;

namespace MyRestaurant.Areas.User.Controllers
{
    [Area("User")]
    public class OrderDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Demo/OrderDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.OrderDetail.Include(o => o.MenuItem);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Demo/OrderDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail
                .Include(o => o.MenuItem)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: Demo/OrderDetails/Create
        public IActionResult Create()
        {
            OrderDetail orderDetail = new OrderDetail();
            ViewData["MenuItemId"] = new SelectList(_context.MenuItem, "MenuItemId", "MenuItemName");
            return View(orderDetail);
        }

        // POST: Demo/OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Quantity,CustomerId,CustomerName,Address,OrderDateTime,MenuItemId")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuItemId"] = new SelectList(_context.MenuItem, "MenuItemId", "MenuItemName", orderDetail.MenuItemId);
            return View(orderDetail);
        }

        // GET: Demo/OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            ViewData["MenuItemId"] = new SelectList(_context.MenuItem, "MenuItemId", "MenuItemName", orderDetail.MenuItemId);
            return View(orderDetail);
        }

        // POST: Demo/OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Quantity,CustomerId,CustomerName,Address,OrderDateTime,MenuItemId")] OrderDetail orderDetail)
        {
            if (id != orderDetail.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.CustomerId))
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
            ViewData["MenuItemId"] = new SelectList(_context.MenuItem, "MenuItemId", "MenuItemName", orderDetail.MenuItemId);
            return View(orderDetail);
        }

        // GET: Demo/OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail
                .Include(o => o.MenuItem)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: Demo/OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderDetail = await _context.OrderDetail.FindAsync(id);
            _context.OrderDetail.Remove(orderDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetail.Any(e => e.CustomerId == id);
        }
    }
}
