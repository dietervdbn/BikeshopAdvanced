using BikeshopAdvanced.Models;
using System.Collections.Generic;
using System.Linq;

namespace BikeshopAdvanced.Data
{
    public class DbInitializer
    {
        public static void Seed(ShopDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Customers.Any() && context.Products.Any())
            {
                return;
            }
            if (!context.Customers.Any())
            {
                var CustommerList = new List<Customer>()
            {
            new Customer {FirstName="Koenraad",Name="DeBlauwe"},
            new Customer {FirstName="Vincent",Name="Tamboryn"},
            new Customer {FirstName="Wim",Name="Forton"},
            new Customer {FirstName="Arthur",Name="Devresse"},
            new Customer {FirstName="Wouter",Name="Verhoeven"},
            new Customer {FirstName="Matthijs",Name="Debacker"},
            new Customer {FirstName="Michiel",Name="Van gasse"},
            };
                foreach (var custommer in CustommerList)
                {
                    context.Customers.Add(custommer);
                }
                context.SaveChanges();
            }

            if (!context.Products.Any())
            {
                var ProductList = new List<Product>()
            {
            new Product {Price= decimal.Parse("550"),Name="fiets"},
            new Product {Price= decimal.Parse("600"),Name="Ps5"},
            new Product {Price= decimal.Parse("40"),Name="Shirt"},
            new Product {Price= decimal.Parse("850"),Name="laptop"},
            new Product {Price= decimal.Parse("1250"),Name="Desktop"},
            new Product {Price= decimal.Parse("250"),Name="scherm"},
            new Product {Price= decimal.Parse("350"),Name="bow"},
            new Product {Price= decimal.Parse("650"),Name="airco"},

            };
                foreach (var product in ProductList)
                {
                    context.Products.Add(product);
                }
                context.SaveChanges();
            }
        }
    }
}
