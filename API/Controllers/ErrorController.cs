using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
   
    public class ErrorController : BaseController
    {
        [HttpGet("/error")]
        public IActionResult Error()
        {
            // Only works in production mode
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            _ = context.Error.StackTrace;
            _ = context.Error.Message;

            return Problem();
        }
    }
}
