using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Models
{
	public class User
	{
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public virtual ICollection<MealRecord> MealRecords { get; set; }
    }
}
