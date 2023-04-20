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
            _context.Add(phoneBookEntry);
            Save();
        }

        public void Add(string name, string phoneNumber)
        {
            var phoneBookEntry = new PhoneBookEntry()
            {
                Id = Guid.NewGuid(),
                Name = name,
                PhoneNumber = phoneNumber
            };

            _context.PhoneBook.Add(phoneBookEntry);
            throw new NotImplementedException();
        }

        public void DeleteByName(string name)
        {
            throw new NotImplementedException();
        }

        public void DeleteByNumber(string number)
        {
            throw new NotImplementedException();
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

