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

        [Required(ErrorMessage = "Name is mandatory.")]
        [StringLength(256, ErrorMessage = "Name is too long.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is mandatory.")]
        [StringLength(64000, ErrorMessage = "Description is too long.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public bool HasQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be positive.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is mandatory.")]
        [RegularExpression(@"^\d*\.?\d*$", ErrorMessage = "Price must be a decimal positive number.")]
        public float PriceInMu { get; set; }

        [Required(ErrorMessage = "Currency is mandatory.")]
        [RegularExpression(@"^RON|EUR|USD$", ErrorMessage = "Unkown currency.")]
        [StringLength(5, ErrorMessage = "Currency name is too long.")]
        public string Currency { get; set; }
        
        // public string Image { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }

        [Required(ErrorMessage = "At least one category is mandatory.")]
        public int[] SelectedCategoryIds { get; set; }

    }
}