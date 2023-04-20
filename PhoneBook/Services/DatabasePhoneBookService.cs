using System.Xml.Linq;
using PhoneBook.Exceptions;
using PhoneBook.Model;

namespace PhoneBook.Services
{
	public class DatabasePhoneBookService : IPhoneBookService
    {
		private readonly PhoneBookContext _context;

		public DatabasePhoneBookService(PhoneBookContext context)
		{
			_context = context;
		}

        public void Add(PhoneBookEntry phoneBookEntry)
        {
            // TODO: check if is a valid PhoneBookEntry
            if (phoneBookEntry.Name == null || phoneBookEntry.PhoneNumber == null)
            {
                throw new ArgumentException("Name and phone number must both be specified.");
            }

            phoneBookEntry.Id = Guid.NewGuid();
            _context.Add(phoneBookEntry);
            Console.WriteLine("Added phoneBookEntry.");
            Save();
        }

        public void Add(string name, string phoneNumber)
        {
            if (name == null || phoneNumber == null)
            {
                throw new ArgumentException("Name and phone number must both be specified.");
            }

            var phoneBookEntry = new PhoneBookEntry()
            {
                Id = Guid.NewGuid(),
                Name = name,
                PhoneNumber = phoneNumber
            };

            //_context.PhoneBook.Add(phoneBookEntry);
            Console.WriteLine("Added name and phoneNumber.");
            throw new NotImplementedException();
        }

        public void DeleteByName(string name)
        {
            var entity = _context.PhoneBook.Where(p => p.Name == name).FirstOrDefault();
            if (entity == null)
            {
                throw new NotFoundException($"No phonebook entry found containing name {name}.");
            }

            _context.Remove(entity);
            Save();
        }

        public void DeleteByNumber(string number)
        {
            var entity = _context.PhoneBook.Where(p => p.PhoneNumber == number).FirstOrDefault();
            if (entity == null)
            {
                throw new NotFoundException($"No phonebook entry found containing phone number {number}.");
            }

            _context.Remove(entity);
            Save();
        }

        public IEnumerable<PhoneBookEntry> List()
        {
            return _context.PhoneBook.OrderBy(p => p.Id).ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0 ? true : false;
        }

    }
}

