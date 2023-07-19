using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus_Manegement.Helper;
using Nexus_Manegement.Models;

namespace Nexus_Manegement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeT7Controller : Controller
    {
        //private NexusContext context = new NexusContext();
        private readonly IDbContextFactory<NexusDbContext> _contextFactory;
        public EmployeeT7Controller(IDbContextFactory<NexusDbContext> dbContextFactory)
        {
            _contextFactory = dbContextFactory;
        }
        public IActionResult Index()
        {
            //kiem tra session 
            string checksession = HttpContext.Session.GetString("username");
            if (checksession != null)
            {
                return View();
            }
            else
            {
                ViewBag.mess = "Login Failed";
                return RedirectToAction("index", "Login");
            }



        }
        [HttpGet]
        public List<Position> Cbx_Position()
        {

            using (var conn = _contextFactory.CreateDbContext())
            {
                var jsonResult = conn.Positions.ToList();
                return jsonResult;
            }
        }
        [HttpGet]
        public IActionResult ListEmployee()
        {
            //kiem tra session 
            string checksession = HttpContext.Session.GetString("username");
            if (checksession != null)
            {
                using (var conn = _contextFactory.CreateDbContext())
                {
                    ViewBag.cbx_position = Cbx_Position();
                    var objectMerge = (from e in conn.Employees
                                       join
                                       p in conn.Positions
                                       on e.Position equals p.ID
                                       select new
                                       {
                                           Id = e.ID,
                                           Name = e.Name,
                                           Phone = e.Phone,
                                           Email = e.Email,
                                           Username = e.Username,
                                           Password = e.Password,
                                           Status = e.Status,
                                           Position = p.Name
                                       }).ToList();
                    //var query = conn.Employees.ToList();
                    return Json(objectMerge);
                }
            }
            else
            {
                ViewBag.mess = "Login Failed";
                return RedirectToAction("index", "Login");
            }
            
        }

        //public IActionResult ListEmployeeJsonTemp()
        //{
        //    using (var conn =_contextFactory.CreateDbContext())
        //    {
        //        var query = conn.Employees.ToList();
        //        return Json(query);
        //    }
        //}
        [HttpGet]
        public JsonResult FormEdit(int id)
        {
            using (var conn = _contextFactory.CreateDbContext())
            {
                var query = conn.Employees.Find(id);
                if (query != null)
                {
                    return new JsonResult(query);

                }
                return new JsonResult(false);
            }
        }
        [HttpPost]
        public async Task<JsonResult> AddOrUpdate(int idPosition, int idEmployee, string fullname, string phone, string email, string username, string password, bool status)
        {
            if (idEmployee == 0)
            {

                Employee e = new Employee
                {
                    Email = email,
                    Name = fullname,
                    Phone = phone,
                    Password = HashPassword.GetInstance.HashPass(password),
                    Username = username,
                    Position = idPosition,
                    Status = status,
                    OTP =0


                };
                try
                {
                    using (var conn = _contextFactory.CreateDbContext())
                    {
                        await conn.Employees.AddAsync(e);
                        await conn.SaveChangesAsync();
                    }
                    return new JsonResult("success");
                }
                catch (Exception)
                {
                    return new JsonResult(false);
                    throw;
                }

            }
            else
            {
                try
                {
                    using (var conn = _contextFactory.CreateDbContext())
                    {
                        var ObjectUpdate = conn.Employees.Find(idEmployee);
                        if (ObjectUpdate != null)
                        {
                            ObjectUpdate.Position = idPosition;
                            ObjectUpdate.Status = status;
                            ObjectUpdate.Phone = phone;
                            ObjectUpdate.Name = fullname;
                            ObjectUpdate.Email = email;
                            ObjectUpdate.Username = username;
                            ObjectUpdate.Password = HashPassword.GetInstance.HashPass(password);
                            conn.Employees.Update(ObjectUpdate);
                            conn.SaveChanges();
                        }
                        return new JsonResult("success");

                    }
                }
                catch (Exception)
                {
                    return new JsonResult(false);
                    throw;
                }
            }

        }
        [HttpPost]
        public IActionResult DeleteEmployee(int id)
        {
            using (var conn = _contextFactory.CreateDbContext())
            {
                var ObjectDelete = conn.Employees.Find(id);
                if (ObjectDelete != null)
                {
                    conn.Remove(ObjectDelete);
                    conn.SaveChanges();
                }
                return Ok();
            }
        }
        [HttpPost]
        public IActionResult ActiveOrDisactive(int id)
        {
            using (var conn = _contextFactory.CreateDbContext())
            {
                var query = conn.Employees.Find(id);
                if (query != null)
                {
                    query.Status = !query.Status;
                    conn.Employees.Update(query);
                    conn.SaveChanges();
                }
                return Ok();
            }
        }
        //[HttpPost]
        //public IActionResult DisActive(int id)
        //{
        //    using(var conn =context)
        //    {
        //        var query = conn.Employees.Find(id);
        //        if( query != null)
        //        {
        //            query.Status = !query.Status;
        //        }
        //    }
        //}

        [HttpGet]
        public JsonResult Find(string data)
        {
            string myParam = Request.Query["data"];
            using (var conn = _contextFactory.CreateDbContext())
            {
                var objectMerge = (from e in conn.Employees
                                   join
                                   p in conn.Positions
                                   on e.Position equals p.ID
                                   where (e.Name.Contains(data) || e.Username.Contains(data) || p.Name.Contains(data))
                                   select new
                                   {
                                       Id = e.ID,
                                       Name = e.Name,
                                       Phone = e.Phone,
                                       Email = e.Email,
                                       Username = e.Username,
                                       Password = e.Password,
                                       Status = e.Status,
                                       Position = p.Name
                                   }).ToList();
                //var objectFind = conn.Employees.Where(w => w.Name.Contains(data) || w.Username.Contains(data)).ToList();
                if (objectMerge != null)
                {
                    return Json(objectMerge);
                }
                else
                {
                    return Json(false);
                }
            }
        }


    }
}
