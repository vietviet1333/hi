using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Nexus_Manegement.Areas.Management.Dao;
using Nexus_Manegement.Models;
using NuGet.Protocol;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System;

using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Nexus_Manegement.Helper;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Bibliography;
using System.Text.Json.Nodes;

namespace Nexus_Manegement.Areas.Management.Controllers
{
    [Area("Management")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ManagementController : Controller
    {


        private NexusContext context = new NexusContext();
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public IActionResult cklogin()
        {

            string user = Request.Form["username"];
            string pass = Request.Form["password"];

            var result = context.Employees.Where(s => s.Username.Equals(user)  && s.Status == true).FirstOrDefault();
            
            if (user != null && pass != null && result != null && HashPassword.GetInstance.CheckPass(pass, result.Password))
            {
                if(result.Position == 1)
                {
                    int ide = result.ID;
                    string emaile = result.Email;
                    TempData["succes"] = "login succes";
                    HttpContext.Session.SetString("sessionlogin", "managementlogin");
                    HttpContext.Session.SetInt32("ide", ide);
                    HttpContext.Session.SetString("emaile", emaile);
                    return RedirectToAction("Store");
                }
                else
                {
                    if(result.Position == 2)
                    {
                        return Redirect("/Accountant/Index");
                    }
                    else
                    {
                        return Redirect("/Technical/Index");
                    }
                }

            }
            else
            {
                TempData["faild"] = " wrong username or pass word";
                return RedirectToAction("Login");

            }

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        public IActionResult profile()
        {
            if ((HttpContext.Session.GetString("sessionlogin")) == "managementlogin")
            {
                int ide = ((int)HttpContext.Session.GetInt32("ide"));
                return View(EmployeeDao.Instance.profile(ide));
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public IActionResult editprofile(int id)
        {
            if ((HttpContext.Session.GetString("sessionlogin")) == "managementlogin")
            {
                return View(EmployeeDao.Instance.profile(id));
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public IActionResult Updateprofile(int id)
        {
            string name = Request.Form["name"];
            string username = Request.Form["username"];
            string email = Request.Form["email"];
            string phone = Request.Form["phone"];
            EmployeeDao.Instance.Updateprofile(id, name, phone, username, email);
            TempData["udp"] = "Update success";
            return RedirectToAction("profile");
        }
        public IActionResult Changepass()
        {
            if ((HttpContext.Session.GetString("sessionlogin")) == "managementlogin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public IActionResult Updatepass()
        {
            int ide = ((int)HttpContext.Session.GetInt32("ide"));
            string oldpass = Request.Form["oldpass"];
            string newpass = Request.Form["newpass"];
            string confirmpass = Request.Form["confirmpass"];
            var result = context.Employees.Where(x => x.ID == ide).FirstOrDefault();
            if (result != null)
            {
                if (oldpass != null && HashPassword.GetInstance.CheckPass(oldpass, result.Password) && newpass == confirmpass)
                {
                    string newpassword = HashPassword.GetInstance.HashPass(newpass);
                    EmployeeDao.Instance.Updatepass(ide, newpassword);
                    return RedirectToAction("Logout");
                }
                else
                {
                    TempData["invalid"] = "Invalid";
                    return RedirectToAction("Changepass");
                }
            }
            else
            {
                return RedirectToAction("Changepass");
            }
        }
        public IActionResult forgotpass()
        {
            return View();
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Store()
        {
            if ((HttpContext.Session.GetString("sessionlogin")) == "managementlogin")
            {


                ViewBag.District = DistricDao.Instance.Districts;
                return View(StoreDao.Instance.Stores);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        public IActionResult Addbranch()
        {
            string name = Request.Form["name"];
            int districtid = int.Parse(Request.Form["district"]);
            string ward = Request.Form["ward"];
            bool status = true;

            StoreDao.Instance.Addbranch(name, districtid, ward, status);
            TempData["ab"] = "add success";
            return RedirectToAction("Store");
        }
        public IActionResult Disactive(int Id)
        {
            StoreDao.Instance.Disactive(Id);
            return RedirectToAction("Store");
        }
        public IActionResult Active(int Id)
        {
            StoreDao.Instance.Active(Id);
            return RedirectToAction("Store");
        }
        public IActionResult Editstore(int Id)
        {
            return View(StoreDao.Instance.Edit(Id));
        }
        public IActionResult Updatestore(int Id)
        {
            string name = Request.Form["Edit_name"];
            int disid = int.Parse(Request.Form["Edit_district"]);
            string ward = Request.Form["Edit_ward"];
            StoreDao.Instance.Update(Id, name, disid, ward);
            return RedirectToAction("Store");
        }
        public IActionResult Filter()
        {
            if ((HttpContext.Session.GetString("sessionlogin")) == "managementlogin")
            {
                int disid = int.Parse(Request.Form["district"]);

                if (disid == 13)
                {
                    return RedirectToAction("Store");
                }
                else
                {
                    var di = StoreDao.Instance.Filter(disid);
                    if (di.Count == 0)
                    {
                        TempData["noresult"] = "No result";
                        ViewBag.District = DistricDao.Instance.Districts;
                        return View();
                    }
                    else
                    {
                        TempData["fi"] = di[0].Distric;
                        TempData["fiid"] = disid;
                        ViewBag.District = DistricDao.Instance.Districts;
                        return View(StoreDao.Instance.Filter(disid));
                    }
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpGet]
        public IActionResult Event()
        {
            if ((HttpContext.Session.GetString("sessionlogin")) == "managementlogin")
            {
                return View(BlogDao.Instance.Blogs);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public IActionResult Addevent(IFormFile Image)
        {
            string fileName;
            string name = Request.Form["name"];

            string content = Request.Form["content"];
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image");
                FileInfo fileInfo = new FileInfo(Image.Name);
                fileName = DateTime.Now.Ticks + Image.FileName;
                string Filenamepath = Path.Combine(path, fileName);
                using (var stream = new FileStream(Filenamepath, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }
            }
            catch
            {
                fileName = "Error";
            }
            BlogDao.Instance.Addevent(name, fileName, content);
            TempData["addevent"] = "add blog success";
            return Redirect("/Management/Event");
        }
        public IActionResult seemoreblog(int id)
        {
            if ((HttpContext.Session.GetString("sessionlogin")) == "managementlogin")
            {
                var data = BlogDao.Instance.blog1(id);
                if (data != null)
                {
                    return View(BlogDao.Instance.EditBlog(id));
                }
                else
                {
                    return Redirect("/Management/Event");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public IActionResult AccountClient()
        {
            if ((HttpContext.Session.GetString("sessionlogin")) == "managementlogin")
            {
                return View(CustomerDao.Instance.Customers);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
      
        public IActionResult Addserviceplan()
        {
            if ((HttpContext.Session.GetString("sessionlogin")) == "managementlogin")
            {
                return View(ServicePlanDao.Instance.ser);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public IActionResult InsertServicePlan(IFormFile Image)
        {

            string[] option = Request.Form["1"];
            Array.Sort(option);
            Option op = new Option();
            var sum = "";
            foreach (string i in option)
            {
                int ii = int.Parse(i);
                var result = context.ExpiryDate.Where(x => x.ID.Equals(ii)).FirstOrDefault();
                op.ID = result.ID;
                op.Name = result.Name;
                var o = op.ToJson();
                if (ii < option.Length)
                {
                    sum += o + ",";
                }
                else
                {
                    sum += o;
                }

            }
            var summ = "[" + sum + "]";
            string name = Request.Form["name"];
            decimal price = decimal.Parse(Request.Form["price"]);
            string description = Request.Form["description"];
            bool status = true;
            int categoryid = int.Parse(Request.Form["category"]);
            string fileName;
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image");
                FileInfo fileInfo = new FileInfo(Image.Name);
                fileName = DateTime.Now.Ticks + Image.FileName;
                string Filenamepath = Path.Combine(path, fileName);
                using (var stream = new FileStream(Filenamepath, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }
            }
            catch
            {
                fileName = "Error";
            }
            ServicePlanDao.Instance.Addserviceplan(name, price, description, status, categoryid, fileName, summ);
            TempData["addservice"] = "add service success";

            return RedirectToAction("Addserviceplan");

        }
        [HttpGet]
        public IActionResult newpass()
        {
            return View();
        }
        [HttpPost]
        public IActionResult changepasss()
        {
            int code = int.Parse(Request.Form["code"]);
            string pass = Request.Form["password"];
            string cpass = Request.Form["cpassword"];
            var result = context.Employees.Where(x => x.OTP == code && x.OTP != 0).FirstOrDefault();
            if (code != null && pass != null && result != null && pass == cpass)
            {
                int ide = result.ID;
                string bpass = HashPassword.GetInstance.HashPass(pass);
                EmployeeDao.Instance.Updatepass(ide, bpass);
                EmployeeDao.Instance.Deleteotp(ide);
                return RedirectToAction("Login");
            }
            else
            {
                TempData["in"] = "invalid";
                return RedirectToAction("newpass");
            }
        }
        [HttpPost]
        public IActionResult sendmail()
        {
            Random generator = new Random();
            int r = generator.Next(100000, 999999);
            string receiver = Request.Form["mail"];
            var result = context.Employees.Where(x => x.Email.Equals(receiver) && x.Status == true).FirstOrDefault();
            if (receiver != null && result != null)
            {

                int ide = result.ID;
                int otp = r;
                EmployeeDao.Instance.UpdateOTP(ide, r);
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Nexus", "ntviet1333@gmail.com"));
                email.To.Add(new MailboxAddress("Employee", receiver));
                email.Subject = "Change Pasword";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = "Code:" + r
                };

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, false);
                    smtp.Authenticate("ntviet1333@gmail.com", "ufgqbsdlmzofvgcs");

                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
                TempData["s"] = "Check mail please";
                return RedirectToAction("newpass");
            }

            else
            {
                TempData["inv"] = "invalid";
                return RedirectToAction("forgotpass");
            }


        }
        public IActionResult Detail(int id)
        {
            if ((HttpContext.Session.GetString("sessionlogin")) == "managementlogin")
            {
                var c = context.Customers.Where(x => x.ID == id).ToList();
                if (c.Count() > 0)
                {
                    string namec = c[0].Name;
                    ViewBag.Namec = namec;
                }
                return View(OderDao.Instance.oder(id));
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public IActionResult Activeser(int id)
        {
            ServicePlanDao.Instance.Activeser(id);
            return RedirectToAction("Addserviceplan");
        }
        public IActionResult Disactiveser(int id)
        {
            ServicePlanDao.Instance.Disactiveser(id);
            return RedirectToAction("Addserviceplan");
        }
        public IActionResult Editser(int id)
        {
            if ((HttpContext.Session.GetString("sessionlogin")) == "managementlogin")
            {
                var ch= ServicePlanDao.Instance.Editser(id);
                if(ch != null)
                {
                    return View(ServicePlanDao.Instance.Editser(id));
                }
                else
                {
                    return RedirectToAction("Addserviceplan");
                }
            }
               
            
            else
            {
                return RedirectToAction("Login");
            }
        }
        public IActionResult Updateser(int id, IFormFile Image)
        {
            string[] option = Request.Form["1"];
            Array.Sort(option);
            Option op = new Option();
            var sum = "";
            foreach (string i in option)
            {
                int ii = int.Parse(i);
                var result = context.ExpiryDate.Where(x => x.ID.Equals(ii)).FirstOrDefault();
                op.ID = result.ID;
                op.Name = result.Name;
                var o = op.ToJson();
                if (ii < option.Length)
                {
                    sum += o + ",";
                }
                else
                {
                    sum += o;
                }

            }
            var summ = "[" + sum + "]";
            string name = Request.Form["name"];
            decimal price = decimal.Parse(Request.Form["price"]);
            string description = Request.Form["description"];
            bool status = true;
            int categoryid = int.Parse(Request.Form["category"]);
            string fileName ;
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image");
                FileInfo fileInfo = new FileInfo(Image.Name);
                fileName = DateTime.Now.Ticks + Image.FileName;
                string Filenamepath = Path.Combine(path, fileName);
                using (var stream = new FileStream(Filenamepath, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }
            }
            catch
            {
                fileName = "Error";
            }
            ServicePlanDao.Instance.Updateser(id, name, price, description, categoryid, fileName, summ);

            return RedirectToAction("Addserviceplan");

        }
    }

}

