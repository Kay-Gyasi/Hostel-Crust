
namespace API.Controllers
{
    public interface IJwtController
    {
        string CreateJWT(Users user);
    }
}