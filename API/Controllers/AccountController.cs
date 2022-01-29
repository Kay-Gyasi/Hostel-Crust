namespace API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IConfiguration configuration;

        public AccountController(IUnitOfWork uow, IConfiguration configuration)
        {
            this.uow = uow;
            this.configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginReqDto loginReqDto)
        {
            var user = await uow.UserRepo.Authenticate(loginReqDto.Username, loginReqDto.Password);

            if(user == null)
            {
                return Unauthorized();
            }

            var loginRes = new LoginResDto();

            loginRes.Username = user.FirstName + ' ' + user.LastName;
            loginRes.Token = CreateJWT(user);

            return Ok(loginRes);
        }

        [HttpGet("Jwt")]
        public string CreateJWT(Users user)
        {
            var secretKey = configuration.GetSection("HostelCrustKey").Value;

            //var secretKey = configuration.GetSection("AppSettings:HostelCrustKey").Value;


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var claims = new Claim[]
            {
                //Set as many claimtypes as required

                new Claim(ClaimTypes.Name, (user.FirstName + ' ' + user.LastName)),

                //new Claim(ClaimTypes.Role, "role should be specified in the database and then passed here" +
                //"in the api methods, check the role of the person and then grant permisions accordingly"),

                new Claim(ClaimTypes.NameIdentifier, user.CustomerID.ToString())
            };

            var signingCredentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
