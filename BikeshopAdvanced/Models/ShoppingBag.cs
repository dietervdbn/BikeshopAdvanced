using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikeshopAdvanced.Models
{
    public class ShoppingBag
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public IdentityUser Customer { get; set; }
        public string CustomerId { get; set; }
        public ICollection<ShoppingItem> ShoppingItems { get; set; }
        public ShoppingBag(DateTime date, string customerId)
        {
            CustomerId = customerId;
            Date = date;
        }
        public ShoppingBag()
        {

        }
    }
}
