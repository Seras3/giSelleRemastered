using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace giSelleRemastered.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime Date { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<InvoiceLine> Lines { get; set; }
    }
}