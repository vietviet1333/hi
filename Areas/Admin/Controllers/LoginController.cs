using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Nexus_Manegement.Helper;
using Nexus_Manegement.Models;

namespace Nexus_Manegement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class LoginController : Controller
    {
        private readonly IDbContextFactory<NexusDbContext> _dbContextFactory;
        public LoginController(IDbContextFactory<NexusDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult CheckLogin(string username, string password)
        {
            using (var conn = _dbContextFactory.CreateDbContext())
            {
                var ObjectUser = conn.Admins.Where(w => w.Username == username).FirstOrDefault();
                if (ObjectUser != null)
                {
                    //if(password == ObjectUser.Password)
                    if (HashPassword.GetInstance.CheckPass(password, ObjectUser.Password))
                    {
                        HttpContext.Session.SetString("username", username);
                        return RedirectToAction("Index", "EmployeeT7");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
                else
                {
                    return Json(new { mess = "User not found!!!" });
                }

            }
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email)
        {
            //var token= await emp
            try
            {
                using (var conn = _dbContextFactory.CreateDbContext())
                {
                    Random generator = new Random();
                    int OTP6 = generator.Next(100000, 999999);
                    string StringOTP6 = OTP6.ToString();


                    var ObjectEmail = conn.Admins.Where(w => w.Email == email).FirstOrDefault();
                    if (ObjectEmail != null)
                    {
                        //
                        var email1 = new MimeMessage();

                        email1.From.Add(new MailboxAddress("Sender Name", "ntviet1333@gmail.com"));
                        email1.To.Add(new MailboxAddress("Receiver Name", email));

                        email1.Subject = "OTP reset password";
                        email1.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                        {
                            Text = "<b>kute</b>" +
                            "<br>" +
                            "<span style='color: red; font-size: 30px; text-center:center;'>" + OTP6 + "</span> "

                        };

                        using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                        {
                            smtp.Connect("smtp.gmail.com", 587, false);

                            // Note: only needed if the SMTP server requires authentication
                            smtp.Authenticate("ntviet1333@gmail.com", "ufgqbsdlmzofvgcs");

                            smtp.Send(email1);
                            smtp.Disconnect(true);
                        }
                        //
                        ViewBag.IdEmail = ObjectEmail.ID;
                        //
                        ObjectEmail.OTP = StringOTP6;
                        conn.Admins.Update(ObjectEmail);
                        conn.SaveChanges();
                        //ViewBag.TempEmail = email;
                        return View("NewPassword");
                        //
                    }
                    return BadRequest(new { mess = "Ko co Email" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        //
        [HttpPost]
        public IActionResult NewPassword(string code, string newPassword)
        {
            using (var conn = _dbContextFactory.CreateDbContext())
            {
                var ObjectPassword = conn.Admins.Where(w => w.OTP == code).FirstOrDefault();
                if (ObjectPassword != null)
                {
                    ObjectPassword.Password = HashPassword.GetInstance.HashPass(newPassword);
                    ObjectPassword.OTP = "0";
                    conn.Admins.Update(ObjectPassword);
                    conn.SaveChanges();
                    return RedirectToAction("index", "Login");
                }
                else
                {
                    ViewBag.mess = "OTP failed";
                    return View("NewPassword");
                }

            }
        }

    }
}
