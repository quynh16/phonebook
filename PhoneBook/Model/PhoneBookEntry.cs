using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Model
{
    public class PhoneBookEntry
    {
        public Guid Id { get; set; }

        [Required (ErrorMessage = "Name and phone number must both be specified.")]
        public string? Name { get; set; }

        [Required (ErrorMessage = "Name and phone number must both be specified.")]
        public string? PhoneNumber { get; set; }
    }
}