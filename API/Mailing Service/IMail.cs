
namespace API.Mailing_Service
{
    public interface IMail
    {
        Task SendMail(string customer, string email, string orderNum);
    }
}