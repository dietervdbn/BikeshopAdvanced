using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikeshopAdvanced.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Naam is verplicht!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Voornaam is verplicht!")]
        public string FirstName { get; set; }
        public ICollection<ShoppingBag> ShoppingBags { get; set; }
    }
}
