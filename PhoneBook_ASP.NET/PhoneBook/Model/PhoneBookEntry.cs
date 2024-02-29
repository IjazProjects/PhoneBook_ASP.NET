using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Model
{
    public class PhoneBookEntry
    {
        [Required]
        [RegularExpression(@"^(?!(?:[^'\n]*\'){2})(?!(?:[^’\n]*’){2})([A-Za-z'’]+[,.]?[ ]?|[A-Za-z'’]+[-]?){1,3}$", ErrorMessage = "Name Format is Incorrect")]
        public string? Name { get; set; }
        
        
        [Required]
        [RegularExpression(@"^([1-9]{5})$|^((\+?\d{0,3}?[-.\s]?)?\(?[1-9]\d{0,3}?\)?[-.\s]?(?:(?=.{1,}$)[-.()\s])\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,9})$", ErrorMessage = "Number Format is Incorrect")]
        public string? PhoneNumber { get; set; }
    }
}