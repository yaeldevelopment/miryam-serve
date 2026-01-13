using System.Net;
using System.Net.Mail;

namespace server.Models
{
    public class Helper
    {
        public static async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var fromAddress = new MailAddress("tripyaeleden@gmail.com", "פניית לקוח");
                var toAddress = new MailAddress("y0556722091@gmail.com");
                string fromPassword = "urof ehie vwrt bxmp"; // **וודאי שאת משתמשת בסיסמת אפליקציה**

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,          
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress))
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;


                    await smtp.SendMailAsync(message);
                    Console.WriteLine("✅ המייל נשלח בהצלחה!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ שגיאה בשליחת המייל: {ex.Message}");
                throw new Exception($"❌ שגיאה בשליחת המייל: {ex.ToString()}");
            }
      
    }
    }
}
