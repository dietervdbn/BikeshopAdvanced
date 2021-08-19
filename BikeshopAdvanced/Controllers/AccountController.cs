using BikeshopAdvanced.Data;
using BikeshopAdvanced.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeshopAdvanced.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ShopDbContext _context;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ShopDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public async Task<IActionResult> RegisterUser(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid == true)
            {
                var user = new IdentityUser { UserName = loginViewModel.UserName, PasswordHash = loginViewModel.Password, Email = loginViewModel.Email };
                var result = await _userManager.CreateAsync(user, loginViewModel.Password);

                if (result.Succeeded)
                {
                                    
                    return View("Login");
                }
                else
                {
                    //what if the user couldn't be added?
                    //logging
                    //exception handling
                    //...
                }
            }
            return View("register", loginViewModel);
        }
        public IActionResult Login()
        {
            return View();
        }
        public async Task<IActionResult> Authenticate(LoginViewModel login)
        {
            if (ModelState.IsValid == true)
            {
                var user = await _userManager.FindByNameAsync(login.UserName);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, login.Password, true, false);
                    if (result.Succeeded)
                    {                        
                        return RedirectToAction("Index", "Products");
                    }
                }
            }
            return View("Login", login);
        }
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
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
