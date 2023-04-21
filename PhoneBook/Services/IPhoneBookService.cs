using PhoneBook.Model;

namespace PhoneBook.Services
{
    public interface IPhoneBookService
    {
        bool Add(PhoneBookDTO phoneBookDTO);
        bool Add(string name, string phoneNumber);
        string? DeleteByName(string name);
        string? DeleteByNumber(string number);
        IEnumerable<PhoneBookDTO> List();
    }
}