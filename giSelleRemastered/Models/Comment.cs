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
        public int UserId { get; set; }
        public int Content { get; set; }
        public DateTime Date { get; set; }

        public virtual ApplicationUser User { get; set; }

    }
}