using System.Net;
using System.Net.Mail;

namespace server.Models
{
    public class Helper
    {
        public static async Task SendEmailAsync(string toEmail, string subject, string body, string? url = null)
        {
            try
            {
                var fromAddress = new MailAddress(Environment.GetEnvironmentVariable("mail"), "פניית לקוח");
                var toAddress = new MailAddress(Environment.GetEnvironmentVariable("to_mai"));
                string fromPassword = Environment.GetEnvironmentVariable("pass_mail"); // **וודאי שאת משתמשת בסיסמת אפליקציה**

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




                    // הוספת קובץ מצורף אם קיים
                    if (url != null)
                    {
                        // מוסיפים fl_attachment כדי להוריד את הקובץ
                        string downloadUrl = $"{url}?fl_attachment&version={DateTime.UtcNow.Ticks}";

                        using (WebClient client = new WebClient())
                        {
                            byte[] fileBytes = client.DownloadData(downloadUrl);
                            var fileName = Path.GetFileName(new Uri(url).AbsolutePath); // מקבל את שם הקובץ

                            Attachment attachment = new Attachment(new MemoryStream(fileBytes), fileName);
                            message.Attachments.Add(attachment);
                        }
                    }



                    await smtp.SendMailAsync(message);
                    Console.WriteLine("✅ המייל נשלח בהצלחה!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ שגיאה בשליחת המייל: {ex.ToString()}");
                throw new Exception($"❌ שגיאה בשליחת המייל: {ex.Message}");
            }
      
    }
    }
}
