using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PhoneBook.Model
{
    [Index(nameof(PhoneNumber), IsUnique = true)]
    public class PhoneBookEntry
    {
        public Guid Id { get; set; }

        [Required (ErrorMessage = "Name and phone number must both be specified.")]
        [RegularExpression("^([A-Za-z](’|'))?[A-Za-z]*,?( ([A-Za-z](’|'))?[A-Za-z]*([ -][A-Za-z]{2,}|( [A-Za-z]\\.))?)?$")]
        public string? Name { get; set; }

        [Required (ErrorMessage = "Name and phone number must both be specified.")]
        [RegularExpression("^((\\+?([0-9]{0,3})( [0-1])?((\\([0-9]{3}\\))|( (\\([0-9]{2}\\)) )|( [0-9]{3}) )?)?[0-9]{3}(-| )[0-9]{4})$|^([0-9]{5}(\\.([0-9]{5}))?)$")]
        public string? PhoneNumber { get; set; }
    }
}