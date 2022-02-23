using System.Net;
using System.Net.Mail;

namespace API.Mailing_Service
{
    public class Mail : IMail
    {
        public async Task SendMail(string customer, string email, string orderNum)
        {
            string messageBody, from, password;

            MailMessage message = new MailMessage();

            from = "kaygyasi715@gmail.com";
            password = "Exdoegh715";

            messageBody = $"Hi {customer}, your order with Id {orderNum} has been received. You wil receive an email" +
                $"once your order has been prepared and ready for delivery. Thank you for purchasing from Hostel Crust.";

            message.From = new MailAddress(from);

            message.To.Add(email);

            message.Subject = "Order received successfully";

            message.Body = messageBody;

            SmtpClient client = new SmtpClient("smtp.gmail.com");

            client.EnableSsl = true;
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(from, password);
            await Task.Run(() => client.Send(message));
        }
    }
}
