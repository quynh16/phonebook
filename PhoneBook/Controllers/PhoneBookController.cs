using Microsoft.AspNetCore.Mvc;
using PhoneBook.Exceptions;
using PhoneBook.Model;
using PhoneBook.Services;

namespace PhoneBook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PhoneBookController : ControllerBase
    {
        private readonly IPhoneBookService _phoneBookService;
        private readonly ILogger<PhoneBookController> _logger;

        public PhoneBookController(IPhoneBookService phoneBookService, ILogger<PhoneBookController> logger)
        {
            _phoneBookService = phoneBookService;
            _logger = logger;
        }

        [HttpGet]
        [Route("list")]
        public IEnumerable<PhoneBookEntry> List()
        {
            _logger.LogInformation("Listing phone book entries");
            return _phoneBookService.List();
        }

        [HttpPost]
        [Route("add")] 
        public IActionResult Add([FromBody] PhoneBookEntry newEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _phoneBookService.Add(newEntry);
            _logger.LogInformation("Entry [{Name}, {PhoneNumber}] added", newEntry.Name, newEntry.PhoneNumber);

            return Ok();
        }

        [HttpPut]
        [Route("deleteByName")]
        public IActionResult DeleteByName([FromQuery] string name)
        {
            try
            {
                _phoneBookService.DeleteByName(name);
                _logger.LogInformation("Entry [{Name}] deleted", name);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                _logger.LogError("Failed to delete: Name [{Name}] does not exist", name);
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        [Route("deleteByNumber")]
        public IActionResult DeleteByNumber([FromQuery] string number)
        {
            string? name;

            try
            {
                name = _phoneBookService.DeleteByNumber(number);
                _logger.LogInformation("Entry [{Name}] deleted", name);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                _logger.LogError("Failed to delete: Number [{Number}] does not exist", number);
                return NotFound(ex.Message);
            }
        }
    }
}