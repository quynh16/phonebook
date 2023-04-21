using System;
using System.Xml.Linq;
using PhoneBook.Controllers;
using PhoneBook.Exceptions;
using PhoneBook.Model;

namespace PhoneBook.Services
{
	public class DatabasePhoneBookService : IPhoneBookService
    {
		private readonly PhoneBookContext _context;
        private readonly ILogger<DatabasePhoneBookService> _logger;

        public DatabasePhoneBookService(PhoneBookContext context, ILogger<DatabasePhoneBookService> logger)
		{
			_context = context;
            _logger = logger;
		}

        public bool Add(PhoneBookEntry phoneBookEntry)
        {
            var retVal = false;

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
                if (entity.Name == phoneBookEntry.Name && entity.PhoneNumber != phoneBookEntry.PhoneNumber)
                {
                    _logger.LogInformation("Updating phone number of {Name} from {OldNumber} to {NewNumber}", entity.Name, entity.PhoneNumber, phoneBookEntry.PhoneNumber);
                    entity.PhoneNumber = phoneBookEntry.PhoneNumber;
                }
                else if (entity.PhoneNumber == phoneBookEntry.PhoneNumber && entity.Name != phoneBookEntry.Name)
                {
                    _logger.LogInformation("Updating phone number of {Number} from {OldName} to {NewName}", entity.PhoneNumber, entity.Name, phoneBookEntry.Name);
                    entity.Name = phoneBookEntry.Name;
                }
                else
                {
                    _logger.LogInformation("Entry [{Name}, {Number}] already exists in database", phoneBookEntry.Name, phoneBookEntry.PhoneNumber);
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
                _context.PhoneBook.Add(newEntry);
                retVal = true;
            }

            var saved = Save();
            return retVal;
        }

        public bool Add(string name, string phoneNumber)
        {
            var retVal = false;

            if (name == null || phoneNumber == null)
            {
                throw new ArgumentException("Name and phone number must both be specified.");
            }

            // Check if phone number already exists
            var entity = _context.PhoneBook.FirstOrDefault(p => p.Name == name || p.PhoneNumber == phoneNumber);

            if (entity != null)
            {
                // Update record if it already exists
                if (entity.Name == name && entity.PhoneNumber != phoneNumber)
                {
                    _logger.LogInformation("Updating phone number of {Name} from {OldNumber} to {NewNumber}", entity.Name, entity.PhoneNumber, phoneNumber);
                    entity.PhoneNumber = phoneNumber;
                }
                else if (entity.PhoneNumber == phoneNumber && entity.Name != name)
                {
                    _logger.LogInformation("Updating phone number of {Number} from {OldName} to {NewName}", entity.PhoneNumber, entity.Name, name);
                    entity.Name = name;
                }
                else
                {
                    _logger.LogInformation("Entry [{Name}, {Number}] already exists", name, phoneNumber);
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
                _context.PhoneBook.Add(newEntry);
                retVal = true;
            }

            var saved = Save();
            return retVal;
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

