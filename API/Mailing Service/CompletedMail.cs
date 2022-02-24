using API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace API.Mailing_Service
{
    public class CompletedMailController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IConfiguration config;

        public CompletedMailController(IUnitOfWork uow, IConfiguration config)
        {
            this.uow = uow;
            this.config = config;
        }

        [HttpGet("CompleteMail/{orderNum}")]
        public async Task<IActionResult> SendCompleteMail(string orderNum)
        {
            int userID = uow.OrderRepo.GetCustomerIDByOrderNum(orderNum);
            Users users = await uow.UserRepo.GetUsersById(userID);
            string messageBody, from, password;

            MailMessage message = new MailMessage();

            from = config["Email Address"];
            password = config["Password"];

            messageBody = $"Hi { users.FirstName.Trim() }, your order with ID {orderNum} has been completed and is ready for pickup. " +
                $"Thank you for purchasing from Hostel Crust.";

            message.From = new MailAddress(from);

            message.To.Add(users.Email);

            message.Subject = "Order received successfully";

            message.Body = messageBody;

            SmtpClient client = new SmtpClient("smtp.gmail.com");

            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(from, password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

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
