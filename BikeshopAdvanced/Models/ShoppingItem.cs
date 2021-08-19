using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikeshopAdvanced.Models
{
    public class ShoppingItem
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Prijs is verplicht!")]
        public int Quentity { get; set; }
        public ShoppingBag ShoppingBag { get; set; }
        public int ShoppingBagId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public ShoppingItem()
        {

        }
        public ShoppingItem(int ProdId, int BagId)
        {
            ShoppingBagId = BagId;
            ProductId = ProdId;
        }
    }
}
