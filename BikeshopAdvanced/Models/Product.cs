using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikeshopAdvanced.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Naam is verplicht!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Prijs is verplicht!")]
        public decimal Price { get; set; }
    }
}
