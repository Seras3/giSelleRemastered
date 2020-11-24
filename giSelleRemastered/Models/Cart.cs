using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace giSelleRemastered.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Product> Products { get; set; }

    }
}