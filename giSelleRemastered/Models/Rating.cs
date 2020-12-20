using System.ComponentModel.DataAnnotations;


namespace giSelleRemastered.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public string UserId { get; set; }

        [Range(1, 5)]
        public int Value { get; set; }
        public virtual Product Product { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}