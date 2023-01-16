using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SuggestionAppUI.Models;
public class CreateSuggestionModel
{
   [Required]
   [MaxLength(75)]
   public string Suggestion { get; set; }
   [Required]
   [MinLength(1)]
   [Display(Name = "Category")]
   public string CategroyId { get; set; }
   [MaxLength(500)]
   public string Description { get; set; }
}
