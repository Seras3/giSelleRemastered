using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace giSelleRemastered.Models
{
    public class ProductAttributes
    {
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

        [DefaultValue(false)]
        public bool Accepted { get; set; }

        [ForeignKey("Image")]
        public int ImageId { get; set; }

        public string UserId { get; set; }
    }

    public class Product : ProductAttributes
    {
        [Key]
        public int Id { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual UploadFile Image { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }

    }

    public class ProductWithCategories : ProductAttributes
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "At least one category is mandatory.")]
        public int[] SelectedCategoryIds { get; set; }

        public ApplicationUser User { get; set; }
        public UploadFile Image { get; set; }
        public List<Category> Categories { get; set; }
        public List<Cart> Carts { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Rating> Ratings { get; set; }
    }

    public class ProductView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasQuantity { get; set; }
        public int Quantity { get; set; }
        public float PriceInMu { get; set; }
        public string Currency { get; set; }
        public bool Accepted { get; set; }

        public ApplicationUser User { get; set; }
        public UploadFile Image { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}