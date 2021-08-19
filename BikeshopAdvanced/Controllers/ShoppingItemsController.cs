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
    public class ShoppingItemsController : Controller
    {
        private readonly ShopDbContext _context;

        public ShoppingItemsController(ShopDbContext context)
        {
            _context = context;
        }

        // GET: ShoppingItems
        public async Task<IActionResult> Index()
        {
            var shopDbContext = _context.shoppingItems.Include(s => s.Product).Include(s => s.ShoppingBag);
            return View(await shopDbContext.ToListAsync());
        }

        // GET: ShoppingItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingItem = await _context.shoppingItems
                .Include(s => s.Product)
                .Include(s => s.ShoppingBag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingItem == null)
            {
                return NotFound();
            }

            return View(shoppingItem);
        }

        // GET: ShoppingItems/Create
        public IActionResult Create(int productId)
        {
            var product = _context.Products.Where(a => a.Id == productId).FirstOrDefault();
            ViewData["ProductId"] = product.Id;
            return View();
        }

        // POST: ShoppingItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Quentity,ShoppingBagId,ProductId")] ShoppingItem shoppingItem)
        {
            if (ModelState.IsValid)
            {
                _context.shoppingItems.Add(shoppingItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", shoppingItem.ProductId);
            ViewData["ShoppingBagId"] = new SelectList(_context.shoppingBags, "Id", "Id", shoppingItem.ShoppingBagId);
            return View(shoppingItem);
        }


        // GET: ShoppingItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingItem = await _context.shoppingItems.FindAsync(id);
            if (shoppingItem == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", shoppingItem.ProductId);
            ViewData["ShoppingBagId"] = new SelectList(_context.shoppingBags, "Id", "Id", shoppingItem.ShoppingBagId);
            return View(shoppingItem);
        }

        // POST: ShoppingItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Quentity,ShoppingBagId,ProductId")] ShoppingItem shoppingItem)
        {
            if (id != shoppingItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingItemExists(shoppingItem.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", shoppingItem.ProductId);
            ViewData["ShoppingBagId"] = new SelectList(_context.shoppingBags, "Id", "Id", shoppingItem.ShoppingBagId);
            return View(shoppingItem);
        }

        // GET: ShoppingItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingItem = await _context.shoppingItems
                .Include(s => s.Product)
                .Include(s => s.ShoppingBag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingItem == null)
            {
                return NotFound();
            }

            return View(shoppingItem);
        }

        // POST: ShoppingItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shoppingItem = await _context.shoppingItems.FindAsync(id);
            _context.shoppingItems.Remove(shoppingItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingItemExists(int id)
        {
            return _context.shoppingItems.Any(e => e.Id == id);
        }
    }
}
