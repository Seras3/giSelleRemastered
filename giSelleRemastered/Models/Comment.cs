using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace giSelleRemastered.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }


        [Required(ErrorMessage = "Content is mandatory.")]
        [StringLength(1000, ErrorMessage = "Too many characters.")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public DateTime Date { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Product Product { get; set; }

    }
}