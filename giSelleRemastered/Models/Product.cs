using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace giSelleRemastered.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasQuantity { get; set; }
        public int Quantity { get; set; }
        public float PriceInMu { get; set; }
        public string Currency { get; set; }
        
        // public string Image { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }

        public int[] CategoryIds { get; set; }

    }
}