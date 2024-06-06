using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Models
{
    public class Dish
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
