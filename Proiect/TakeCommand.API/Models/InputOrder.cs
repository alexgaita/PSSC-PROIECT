using System.ComponentModel.DataAnnotations;
using LanguageExt;

namespace TakeCommand.API.Models;

public class InputOrder
{
    [Required]
    public string Address { get; set; }
    
    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    
    [Required]
    public List<InputProduct> Products { get; set; }

    public class InputProduct
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than or equal to 1")]
        public int Id { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than or equal to 1")]
        public int Quantity { get; set; }
    }
}