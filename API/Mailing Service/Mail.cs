using API.Controllers;
using System.Net;
using System.Net.Mail;

namespace API.Mailing_Service
{
    public class MailController : BaseController
    {
        private readonly IUnitOfWork uow;

        public MailController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpGet("SendMail/{customer}/{orderNum}")]
        public async Task<IActionResult> SendMail(string customer, string orderNum)
        {
            Users users = await uow.UserRepo.GetUserByName(customer);
            string messageBody, from, password;

            MailMessage message = new MailMessage();

            from = "kaygyasi715@gmail.com";
            password = "Exdoegh715";

            messageBody = $"Hi {customer}, your order with Id {orderNum} has been received. You wil receive an email" +
                $"once your order has been prepared and ready for delivery. Thank you for purchasing from Hostel Crust.";

            message.From = new MailAddress(from);

            message.To.Add(users.Email);

            message.Subject = "Order received successfully";

            message.Body = messageBody;

            SmtpClient client = new SmtpClient("smtp.gmail.com");

            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(from, password);

            try
            {
                client.Send(message);
                return Ok("Mail sent succesfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
    }
}
