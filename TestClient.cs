using MimeKit;
using System;

using MailKit.Net.Smtp;
using MailKit;


namespace Nexus_Manegement
{
    public class TestClient
    {


        public static void Main(string[] args)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("Sender Name", "sender@email.com"));
            email.To.Add(new MailboxAddress("Receiver Name", "receiver@email.com"));

            email.Subject = "Testing out email sending";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "<b>Hello all the way from the land of C#</b>"
            };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                smtp.Authenticate("smtp_username", "smtp_password");

                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
    }
}

