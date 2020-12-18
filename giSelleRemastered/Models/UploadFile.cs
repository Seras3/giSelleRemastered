using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace giSelleRemastered.Models
{
    public class UploadFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FileId { get; set; }

        public string Name { get; set; }
        public string Extension { get; set; }
    }
}