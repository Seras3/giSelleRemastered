using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace giSelleRemastered.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Name is mandatory.")]
        [Index(IsUnique = true)]
        [StringLength(256, ErrorMessage = "Name is too long.")]
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}