using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BikeshopAdvanced.Data;
using BikeshopAdvanced.Models;

namespace BikeshopAdvanced
{
    public class ShoppingBagsController : Controller
    {
        private readonly ShopDbContext _context;

        public ShoppingBagsController(ShopDbContext context)
        {
            _context = context;
        }

        // GET: ShoppingBags
        public async Task<IActionResult> Index()
        {
            var shopDbContext = _context.shoppingBags.Include(s => s.Customer);
            return View(await shopDbContext.ToListAsync());
        }

        // GET: ShoppingBags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingBag = await _context.shoppingBags
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingBag == null)
            {
                return NotFound();
            }

            return View(shoppingBag);
        }

        // GET: ShoppingBags/Create
        public IActionResult Create(int? ClientId)
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FirstName");
            var client = _context.Customers.Where(a => a.Id == ClientId).FirstOrDefault();
            ViewData["ClientId"] = client.Id;
            return View();
        }

        // POST: ShoppingBags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,CustomerId")] ShoppingBag shoppingBag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingBag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FirstName", shoppingBag.CustomerId);
            return View(shoppingBag);
        }

        // GET: ShoppingBags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingBag = await _context.shoppingBags.FindAsync(id);
            if (shoppingBag == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FirstName", shoppingBag.CustomerId);
            return View(shoppingBag);
        }

        // POST: ShoppingBags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,CustomerId")] ShoppingBag shoppingBag)
        {
            if (id != shoppingBag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingBag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingBagExists(shoppingBag.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FirstName", shoppingBag.CustomerId);
            return View(shoppingBag);
        }

        // GET: ShoppingBags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingBag = await _context.shoppingBags
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingBag == null)
            {
                return NotFound();
            }

            return View(shoppingBag);
        }

        // POST: ShoppingBags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shoppingBag = await _context.shoppingBags.FindAsync(id);
            _context.shoppingBags.Remove(shoppingBag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingBagExists(int id)
        {
            return _context.shoppingBags.Any(e => e.Id == id);
        }
    }
}
