using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DR_Lista.Validation;

namespace DR_Lista.Models
{
    public class ShoppingList
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [FutureOrCurrentDate]
        public DateTime Date { get; set; }

        public ICollection<ShoppingItem> ShoppingItems { get; set; } = new List<ShoppingItem>();

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public User User { get; set; }
    }
}