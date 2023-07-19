using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Nexus_Management.Areas.Accountant.Dao;
using Nexus_Management.Areas.Technical.ModelsView;

using Nexus_Manegement.Areas.Technical.Dao;
using Nexus_Manegement.Models;

namespace Nexus_Management.Areas.Technical.Controllers
{
    [Area("Technical")]
    public class TechnicalController : Controller
    {
        //
        public IActionResult Index()
        {
            //session id
            var model = TechnicalDao.Instance.getEmp(1);
            return View(model);
        }
        public IActionResult ChangeProfile(string name, string username, string email, string phone)
        {
            //session id
            TechnicalDao.Instance.changeProfile(1, name, username, email, phone);
            return RedirectToAction("Index");
        }

        public IActionResult ChangePass(string newPass)
        {
            //session id
            TechnicalDao.Instance.changePassword(1, newPass);
            return RedirectToAction("Index");
        }
        public IActionResult Order(int? pageNumber)
        {
            TechnicalDao.Instance.CountOrderExpiry();

            ViewBag.Year = TechnicalDao.Instance.GetYear();
            TechnicalDao.Instance.CheckConnect();
            var model = TechnicalDao.Instance.getOrder();

           
            return View(model);
            //return View(list);
        }
        //public IActionResult SearchIdOrder(string searchString)
        //{
        //    var order = TechnicalDao.Instance.getOrder();
        //    var orderSearch = TechnicalDao.Instance.getOrderWithSelect();
        //    if (!string.IsNullOrEmpty(searchString)) {
        //        orderSearch= orderSearch.Where(x=>x.IDOrder.Equals(searchString)).ToList();
        //    }
        //    return PartialView("_OrderTechnical",orderSearch);
        //}

        //public IActionResult ShowOrderSelect(string year, string month, int? pageNumber)
        //{
        //    var order = TechnicalDao.Instance.getOrder();
        //    var orderSelect = TechnicalDao.Instance.getOrderWithSelect();

        //    int pageSize = 5;
        //    var list = PaginatedList<OrderView>.Create(order, pageNumber ?? 1, pageSize);

        //    if (!string.IsNullOrEmpty(year) && year != DateTime.Now.Year.ToString())
        //    {
        //        if (!string.IsNullOrEmpty(month) && month != "all")
        //        {
        //            orderSelect = orderSelect.Where(x => x.OrderYear == year && x.OrderDate == month).ToList();
        //            int pageSizeSelect = 5;
        //            var listSelect = PaginatedList<OrderView>.Create(orderSelect, pageNumber ?? 1, pageSizeSelect);

        //            return PartialView("_OrderTechnical", listSelect);
        //        }
        //        else
        //        {
        //            orderSelect = orderSelect.Where(x => x.OrderYear == year).ToList();
        //            int pageSizeSelect = 5;
        //            var listSelect = PaginatedList<OrderView>.Create(orderSelect, pageNumber ?? 1, pageSizeSelect);
        //            return PartialView("_OrderTechnical", listSelect);
        //        }
        //    }
        //    else
        //    {
        //        if (!string.IsNullOrEmpty(month) && month != "all")
        //        {
        //            order = order.Where(x => x.OrderDate == month).ToList();

        //            var listMonth = PaginatedList<OrderView>.Create(order, pageNumber ?? 1, pageSize);
        //            if (order.Count == 0)
        //            {
        //                TempData["Message"] = "No result for this month.";
        //                return PartialView("_OrderTechnical", list);
        //            }
        //            else
        //            {
        //                return PartialView("_OrderTechnical", listMonth);
        //            }
        //        }
        //        else
        //        {

        //            return PartialView("_OrderTechnical", list);
        //        }
        //    }
        //}
        public IActionResult ShowOrderSelect(string year, string month, int? pageNumber)
        {
            var order = TechnicalDao.Instance.getOrder();
            var orderSelect = TechnicalDao.Instance.getOrderWithSelect();

            if (!string.IsNullOrEmpty(year) && year != DateTime.Now.Year.ToString())
            {
                if (!string.IsNullOrEmpty(month) && month != "all")
                {
                    orderSelect = orderSelect.Where(x => x.OrderYear == year && x.OrderDate == month).ToList();
                    return PartialView("_OrderTechnical", orderSelect);
                }
                else
                {
                    orderSelect = orderSelect.Where(x => x.OrderYear == year).ToList();
                    return PartialView("_OrderTechnical", orderSelect);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(month) && month != "all")
                {
                    order = order.Where(x => x.OrderDate == month).ToList();
                    if (order.Count == 0)
                    {
                        TempData["Message"] = "No result for this month.";
                        return PartialView("_OrderTechnical", order);
                    }
                    else
                    {
                        return PartialView("_OrderTechnical", order);
                    }
                }
                else
                {
                    return PartialView("_OrderTechnical", order);
                }
            }
        }
        public int CountOrderExpiry()
        {
            return TechnicalDao.Instance.CountOrderExpiry();
        }
        public IActionResult _MessengerOrderExpired()
        {
            ViewBag.model = TechnicalDao.Instance.MessOrderExpiry();
            return PartialView();
        }
        public IActionResult AboutToExpire()
        {
            var model = TechnicalDao.Instance.MessOrderExpiry();

            return View(model);
        }
        public IActionResult SendMailAboutToExpire(string mail, int timeSpan, string service)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("Nexus Service Marketing System", "ntviet1333@gmail.com"));
            email.To.Add(new MailboxAddress("Receiver Name", mail));

            email.Subject = "About To Expire";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "<b>Your " + service + " has " + timeSpan + " more days to expire. Please visit the homepage for further extension.</b>"
            };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                smtp.Authenticate("ntviet1333@gmail.com", "ufgqbsdlmzofvgcs");

                smtp.Send(email);
                smtp.Disconnect(true);
            }

            return RedirectToAction("AjSendMailActionReturn");

        }

        public IActionResult AjSendMailActionReturn()
        {
            ViewBag.lsData= TechnicalDao.Instance.MessOrderExpiry();
            return PartialView("PartAboutToExpire");
        }
        

        public IActionResult NewConnect(int id, int idOrder, int usagepack)
        {
            TechnicalDao.Instance.NewConnect(id, idOrder, usagepack);
            return RedirectToAction("Order");

        }
        public IActionResult Exchange()
        {
            var model = TechnicalDao.Instance.Exchange();
            return View(model);
        }
        public IActionResult SaveExchange(int id)
        {
            TechnicalDao.Instance.SaveExchange(id);
            return RedirectToAction("Exchange");
        }

    }
}
