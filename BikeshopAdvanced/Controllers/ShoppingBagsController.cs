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

        public async Task<IActionResult> Index()
        {
            var shopDbContext = _context.shoppingBags.Include(s => s.Customer);
            return View(await shopDbContext.ToListAsync());
        }
    }
}
