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

        public bool Add(PhoneBookDTO phoneBookDTO)
        {
            // TODO: check if is a valid PhoneBookEntry
            if (phoneBookDTO.Name == null || phoneBookDTO.PhoneNumber == null)
            {
                throw new ArgumentException("Name and phone number must both be specified.");
            }

            // Check if phone number already exists
            var entity = _context.PhoneBook.FirstOrDefault(p => p.PhoneNumber == phoneBookDTO.PhoneNumber);

            if (entity != null)
            {
                // Update record with new name if phone number already exists
                System.Diagnostics.Debug.WriteLine($"Updating owner of phone number {phoneBookDTO.PhoneNumber} from \"{entity.Name}\" to \"{phoneBookDTO.Name}\".");
                entity.Name = phoneBookDTO.Name;
            }
            else
            {
                // Create a new PhoneBookEntry with a random GUID
                PhoneBookEntry newEntry = new PhoneBookEntry
                {
                    Id = Guid.NewGuid(),
                    Name = phoneBookDTO.Name,
                    PhoneNumber = phoneBookDTO.PhoneNumber
                };

                System.Diagnostics.Debug.WriteLine($"Adding {newEntry.Name} as the owner of phone number {newEntry.PhoneNumber}.");
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
            var entity = _context.PhoneBook.FirstOrDefault(p => p.PhoneNumber == phoneNumber);

            if (entity != null)
            {
                // Update record with new name if phone number already exists
                System.Diagnostics.Debug.WriteLine($"Updating owner of phone number {phoneNumber} from \"{entity.Name}\" to \"{name}\".");
                entity.Name = name;
            }
            else
            {
                // Add new record
                var phoneBookEntry = new PhoneBookEntry()
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    PhoneNumber = phoneNumber
                };

                System.Diagnostics.Debug.WriteLine($"Adding {name} as the owner of phone number {phoneNumber}.");
                _context.PhoneBook.Add(phoneBookEntry);
            }

            var saved = Save();
            return saved;
        }

        public string? DeleteByName(string name)
        {
            var entity = _context.PhoneBook.Where(p => p.Name == name).FirstOrDefault();
            if (entity == null)
            {
                throw new NotFoundException($"No phonebook entry found containing name {name}.");
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
                throw new NotFoundException($"No phonebook entry found containing phone number {number}.");
            }

            var name = entity.Name;
            _context.Remove(entity);
            var saved = Save();

            return saved ? name : null;
        }

        public IEnumerable<PhoneBookDTO> List()
        {
            return _context.PhoneBook.Select(x => new PhoneBookDTO
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

