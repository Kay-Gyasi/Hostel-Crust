namespace API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IJwtController jwt;
        private readonly IDIFactory factory;

        public AccountController(IUnitOfWork uow, IJwtController jwt, IDIFactory factory)
        {
            this.uow = uow;
            this.jwt = jwt;
            this.factory = factory;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginReqDto loginReqDto)
        {
            var user = await uow.UserRepo.Authenticate(loginReqDto.Email, loginReqDto.Password);

            if(user == null)
            {
                return Unauthorized();
            }

            var loginRes = factory.LoginResDto();

            loginRes.Username = user.FirstName + ' ' + user.LastName;
            loginRes.Token = jwt.CreateJWT(user);

            return Ok(loginRes);
        }
    }
}
