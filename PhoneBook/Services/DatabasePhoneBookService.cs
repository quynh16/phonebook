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
            throw new NotImplementedException();
        }

        public void Add(string name, string phoneNumber)
        {
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
    }
}

