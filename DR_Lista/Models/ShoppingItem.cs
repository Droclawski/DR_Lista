using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DR_Lista.Models
{
    public class ShoppingItem
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        public bool IsChecked { get; set; }

        public int? ShoppingListId { get; set; }
        [ForeignKey("ShoppingListId")]
        [ValidateNever]
        public ShoppingList ShoppingList { get; set; }

        [BindNever]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public User User { get; set; }
    }
}
