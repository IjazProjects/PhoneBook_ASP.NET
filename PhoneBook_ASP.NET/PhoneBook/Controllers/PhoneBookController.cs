using Microsoft.AspNetCore.Mvc;
using PhoneBook.Exceptions;
using PhoneBook.Model;
using PhoneBook.Services;
using System.ComponentModel.DataAnnotations;
using Serilog;
using System.Xml.Linq;

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
            _logger.LogInformation("Pulled Information from List");
            return _phoneBookService.List();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add([FromBody]PhoneBookEntry newEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _phoneBookService.Add(newEntry);
            _logger.LogInformation("Added the new name: {newEntry}",newEntry.Name);
            return Ok();
        }

        [HttpPut]
        [Route("deleteByName")]
        public IActionResult DeleteByName([FromQuery,RegularExpression(@"^(?!(?:[^'\n]*\'){2})(?!(?:[^’\n]*’){2})([A-Za-z'’]+[,.]?[ ]?|[A-Za-z'’]+[-]?){1,3}$", ErrorMessage = "Name Format is Incorrect")] string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _phoneBookService.DeleteByName(name);
                _logger.LogInformation("Removed by name from: {newEntry}", name);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        [Route("deleteByNumber")]
        public IActionResult DeleteByNumber([FromQuery, RegularExpression(@"^([1-9]{5})$|^((\+?\d{0,3}?[-.\s]?)?\(?[1-9]\d{0,3}?\)?[-.\s]?(?:(?=.{1,}$)[-.()\s])\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,9})$", ErrorMessage = "Number Format is Incorrect")] string number)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var name = _phoneBookService.DeleteByNumber(number);
                _logger.LogInformation("Removed from Number by: {newEntry}", name);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}