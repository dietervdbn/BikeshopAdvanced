using BikeshopAdvanced.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeshopAdvanced.Data
{
    public class ShopDbContext : IdentityDbContext<IdentityUser>
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ShoppingBag> shoppingBags { get; set; }
        public DbSet<ShoppingItem> shoppingItems { get; set; }
    }
}
