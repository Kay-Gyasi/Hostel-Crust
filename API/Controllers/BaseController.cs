namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        // Retrieving user info from authentication token
        protected int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        protected string GetUserName()
        {
            return User.FindFirst(ClaimTypes.Name).Value;
        }
    }  
}
