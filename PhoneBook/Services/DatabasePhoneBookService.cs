using System;
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

        public bool Add(PhoneBookEntry phoneBookEntry)
        {
            // TODO: check if is a valid PhoneBookEntry
            if (phoneBookEntry.Name == null || phoneBookEntry.PhoneNumber == null)
            {
                throw new ArgumentException("Name and phone number must both be specified.");
            }

            // Check if phone number already exists
            var entity = _context.PhoneBook.FirstOrDefault(p => p.Name == phoneBookEntry.Name || p.PhoneNumber == phoneBookEntry.PhoneNumber);

            if (entity != null)
            {
                // Update record if it already exists
                if (entity.Name == phoneBookEntry.Name)
                {
                    System.Diagnostics.Debug.WriteLine($"Updating phone number of \"{entity.Name}\" from {entity.PhoneNumber} to {phoneBookEntry.PhoneNumber}");
                    entity.PhoneNumber = phoneBookEntry.PhoneNumber;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Updating owner of phone number {phoneBookEntry.PhoneNumber} from \"{entity.Name}\" to \"{phoneBookEntry.Name}\"");
                    entity.Name = phoneBookEntry.Name;
                }
            }
            else
            {
                // Create a new PhoneBookEntry with a random GUID
                PhoneBookEntryDB newEntry = new PhoneBookEntryDB
                {
                    Id = Guid.NewGuid(),
                    Name = phoneBookEntry.Name,
                    PhoneNumber = phoneBookEntry.PhoneNumber
                };

                System.Diagnostics.Debug.WriteLine($"Adding {newEntry.Name} as the owner of phone number {newEntry.PhoneNumber}");
                _context.PhoneBook.Add(newEntry);
            }

            var saved = Save();
            return saved;
        }

        public bool Add(string name, string phoneNumber)
        {
            if (name == null || phoneNumber == null)
            {
                throw new ArgumentException("Name and phone number must both be specified.");
            }

            // Check if phone number already exists
            var entity = _context.PhoneBook.FirstOrDefault(p => p.Name == name || p.PhoneNumber == phoneNumber);

            if (entity != null)
            {
                // Update record if it already exists
                if (entity.Name == name)
                {
                    System.Diagnostics.Debug.WriteLine($"Updating phone number of \"{entity.Name}\" from {entity.PhoneNumber} to {phoneNumber}");
                    entity.PhoneNumber = phoneNumber;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Updating owner of phone number {phoneNumber} from \"{entity.Name}\" to \"{name}\"");
                    entity.Name = name;
                }
            }
            else
            {
                // Add new record
                var newEntry = new PhoneBookEntryDB()
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    PhoneNumber = phoneNumber
                };

                System.Diagnostics.Debug.WriteLine($"Adding \"{name}\" as the owner of phone number {phoneNumber}");
                _context.PhoneBook.Add(newEntry);
            }

            var saved = Save();
            return saved;
        }

        public string? DeleteByName(string name)
        {
            var entity = _context.PhoneBook.Where(p => p.Name == name).FirstOrDefault();
            if (entity == null)
            {
                throw new NotFoundException($"No phonebook entry found containing name \"{name}\"");
            }

            _context.Remove(entity);
            var saved = Save();

            return saved ? name : null;
        }

        public string? DeleteByNumber(string number)
        {
            var entity = _context.PhoneBook.Where(p => p.PhoneNumber == number).FirstOrDefault();
            if (entity == null)
            {
                throw new NotFoundException($"No phonebook entry found containing phone number {number}");
            }

            var name = entity.Name;
            _context.Remove(entity);
            var saved = Save();

            return saved ? name : null;
        }

        public IEnumerable<PhoneBookEntry> List()
        {
            return _context.PhoneBook.Select(x => new PhoneBookEntry
            {
                Name = x.Name, PhoneNumber = x.PhoneNumber
            });
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0 ? true : false;
        }

    }
}

