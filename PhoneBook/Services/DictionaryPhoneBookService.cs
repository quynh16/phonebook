 using PhoneBook.Exceptions;
using PhoneBook.Model;

namespace PhoneBook.Services
{
    public class DictionaryPhoneBookService : IPhoneBookService
    {
        private readonly Dictionary<string, string> _phoneBookEntries;

        public DictionaryPhoneBookService()
        {
            _phoneBookEntries = new Dictionary<string, string>();
        }

        public bool Add(PhoneBookDTO phoneBookDTO)
        {
            if (phoneBookDTO.Name == null || phoneBookDTO.PhoneNumber == null)
            {
                throw new ArgumentException("Name and phone number must both be specified.");
            }

            _phoneBookEntries.Add(phoneBookDTO.Name, phoneBookDTO.PhoneNumber);

            return true;
        }

        public bool Add(string name, string phoneNumber)
        {
            if (name == null || phoneNumber == null)
            {
                throw new ArgumentException("Name and phone number must both be specified.");
            }

            _phoneBookEntries.Add(name, phoneNumber);

            return true;
        }

        public IEnumerable<PhoneBookDTO> List()
        {
            List<PhoneBookDTO> entriesList = new List<PhoneBookDTO>();

            foreach (var name in _phoneBookEntries.Keys)
            {
                entriesList.Add(new PhoneBookDTO { Name = name, PhoneNumber = _phoneBookEntries[name] });
            }

            return entriesList;
        }

        public string? DeleteByName(string name)
        {
            if (!_phoneBookEntries.ContainsKey(name))
            {
                throw new NotFoundException($"No phonebook entry found containing name {name}.");
            }

            _phoneBookEntries.Remove(name);
            return name;
        }

        public string? DeleteByNumber(string number)
        {
            var name = _phoneBookEntries.Where(kvp => kvp.Value == number).FirstOrDefault().Key;
            if (name == null)
            {
                throw new NotFoundException($"No phonebook entry found containing phone number {number}.");
            }

            _phoneBookEntries.Remove(name);
            return name;
        }
    }
}
