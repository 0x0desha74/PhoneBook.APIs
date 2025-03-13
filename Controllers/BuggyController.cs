using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneBook.APIs.Errors;
using PhoneBook.Repository.Data;

namespace PhoneBook.APIs.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly ApplicationDbContext _context;

        public BuggyController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("notfound")]
        public ActionResult GetNotFound()
        {
            return NotFound(new ApiResponse(404));
        }

        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var book = _context.Contacts.Find(100);
            var bookToReturn = book.ToString();
            return Ok(bookToReturn);
        }
        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }

    }
}
