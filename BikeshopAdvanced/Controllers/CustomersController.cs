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
    public class CustomersController : Controller
    {
        private readonly ShopDbContext _context;

        public CustomersController(ShopDbContext context)
        {
            _context = context;
        }

        // GET: Customers

        public async Task<IActionResult> Index(string? sortOrder, string? currentFilter, string? searchString, int? pageNumber)

        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["Sorteer op familienaam"] = String.IsNullOrEmpty(sortOrder) ? "name" : "";
            ViewData["Sorteer op voornaam"] = sortOrder == "Lasname" ? "Lasname" : "Lasname";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["SearchFilter"] = searchString;
            var customer = from s in _context.Customers
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                customer = customer.Where(s => s.Name.Contains(searchString)
                                       || s.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name":
                    customer = customer.OrderBy(s => s.Name);
                    break;
                case "Lasname":
                    customer = customer.OrderBy(s => s.FirstName);
                    break;
                default:
                    customer = customer.OrderBy(s => s.Id);
                    break;
            }
            int pageSize = 5;
            return View(await PaginatedList<Customer>.CreateAsync(customer.AsNoTracking(), pageNumber ?? 1, pageSize));
        }


        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(x => x.ShoppingBags)
                .ThenInclude(x => x.ShoppingItems)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                var shoppingbag = customer.ShoppingBags.FirstOrDefault();
                shoppingbag.ShoppingItems = CompresProducts(shoppingbag.ShoppingItems);
                List < ShoppingBag > lijst = new List<ShoppingBag> { shoppingbag };
                customer.ShoppingBags = lijst;
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,FirstName")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                ShoppingBag shoppingBag = CreateShoppingBag(customer);
                customer.ShoppingBags = _context.shoppingBags.Where(x => x.Id == shoppingBag.Id).ToList();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        private ShoppingBag CreateShoppingBag(Customer customer)
        {
            DateTime today = DateTime.Today;
            ShoppingBag shoppingBag = new ShoppingBag(today, customer.Id);
            _context.shoppingBags.Add(shoppingBag);
            _context.SaveChanges();
            return shoppingBag;
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,FirstName")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
        public IActionResult Bag()
        {

            ShoppingBag shoppingBag = _context.shoppingBags
                .Include(x => x.Customer)
                .Include(x => x.ShoppingItems)
                .ThenInclude(y => y.Product)
                .FirstOrDefault();
            shoppingBag.ShoppingItems = CompresProducts(shoppingBag.ShoppingItems);

            return View(shoppingBag);
        }
        public bool CheckIfAlreadyUsed(ICollection<ShoppingItem> shoppingItems, ShoppingItem shoppingItem)
        {
            bool found = false;
            foreach (var item in shoppingItems)
            {
                if (shoppingItem.ProductId == item.ProductId)
                {
                    found = true;
                }
            }
            return found;
        }
        public int CountItems(ICollection<ShoppingItem> shoppingItems, ShoppingItem shoppingItem)
        {
            int amount = 0;
            foreach (var item in shoppingItems)
            {
                if (shoppingItem.ProductId == item.ProductId)
                {
                    amount++;
                }
            }
            return amount;
        }
        public ICollection<ShoppingItem> MakeCopy(ICollection<ShoppingItem> shoppingItems)
        {
            List<ShoppingItem> shoppingItemsCopy = new List<ShoppingItem>();
            foreach (var item in shoppingItems)
            {
                shoppingItemsCopy.Add(item);
            }
            return shoppingItemsCopy;
        }
        public ICollection<ShoppingItem> CompresProducts(ICollection<ShoppingItem> shoppingItems)
        {
            ICollection<ShoppingItem> shoppingItemsOriginal = MakeCopy(shoppingItems);

            foreach (var item in shoppingItems)
            {
                if (CountItems(shoppingItems, item) > 1)
                {
                    shoppingItems.Remove(item);
                }
            }

            foreach (var item in shoppingItems)
            {
                if (CountItems(shoppingItemsOriginal, item) > 1)
                {
                    item.Quentity = 0;
                    foreach (var itemOriginal in shoppingItemsOriginal)
                    {
                        if (item.ProductId == itemOriginal.ProductId)
                        {
                            item.Quentity += itemOriginal.Quentity;
                        }
                    }

                }
            }
            return shoppingItems;
        }
    }
}