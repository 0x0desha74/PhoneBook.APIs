using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.APIs.Errors;

namespace PhoneBook.APIs.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public IActionResult Error(int code)
        {
            return NotFound(new ApiResponse(404,"Not Found Endpoint"));
        }

    }
}
