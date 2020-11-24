using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace giSelleRemastered.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public int Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}