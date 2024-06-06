using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Models
{
    public class MealRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int DishId { get; set; }

        public virtual User User { get; set; }
        public virtual Dish Dish { get; set; }
    }
}
