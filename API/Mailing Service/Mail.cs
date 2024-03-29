﻿using API.Controllers;
using System.Net;
using System.Net.Mail;

namespace API.Mailing_Service
{
    public class MailController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IConfiguration config;

        public MailController(IUnitOfWork uow, IConfiguration config)
        {
            this.uow = uow;
            this.config = config;
        }

        [HttpGet("SendMail/{orderNum}")]
        public async Task<IActionResult> SendMail(string orderNum)
        {
            int userID = uow.OrderRepo.GetCustomerIDByOrderNum(orderNum);
            Users users = await uow.UserRepo.GetUsersById(userID);
            string messageBody, from, password;

            MailMessage message = new MailMessage();

            from = config["Email Address"];
            password = config["Password"];

            messageBody = $"Hi { users.FirstName.Trim() }, your order with ID {orderNum} has been received. You will receive an email " +
                $"once your order has been prepared and ready for delivery. Thank you for purchasing from Hostel Crust.";

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
