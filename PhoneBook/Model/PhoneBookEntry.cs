using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Model
{
    public class PhoneBookEntry
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }
    }
}