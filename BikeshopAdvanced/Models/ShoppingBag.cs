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
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public ICollection<ShoppingItem> ShoppingItems { get; set; }
        public ShoppingBag(DateTime date, int customerId)
        {
            CustomerId = customerId;
            Date = date;
        }
        public ShoppingBag()
        {

        }
    }
}
