using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus_Manegement.Models;

namespace Nexus_Manegement.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class PositionV3Controller : Controller
    {
        private readonly IDbContextFactory<NexusDbContext> _dbContextFactory;
        public PositionV3Controller(IDbContextFactory<NexusDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public List<Position> Cbx_position()
        {
            using (var conn = _dbContextFactory.CreateDbContext())
            {
                var query = conn.Positions.ToList();
                return query;
            }
        }
        [HttpGet]
        public JsonResult FindById(string data)
        {
            using (var conn = _dbContextFactory.CreateDbContext())
            {
                var query = conn.Positions.Where(w => w.Name.Contains(data)).ToList();
                //if (query == null)
                //{
                return new JsonResult(query);
                //}

            }
        }
        public IActionResult Index()
        {
            using (var conn = _dbContextFactory.CreateDbContext())
            {
                var query = conn.Positions.ToList();
                return View();
            }
        }
        [HttpGet]
        public JsonResult ListPosition()
        {
            using (var conn = _dbContextFactory.CreateDbContext())
            {
                var query = conn.Positions.ToList();
                return new JsonResult(query);
            }
        }
        [HttpGet]
        public JsonResult FormAddOrUpdate(int id)
        {
            if (id == 0)
            {
                //Cbx_position();
                return new JsonResult(Cbx_position());
            }
            else
            {
                using (var conn = _dbContextFactory.CreateDbContext())
                {
                    try
                    {
                        var ObjectEdit = conn.Positions.Find(id);

                        return Json(ObjectEdit);

                    }
                    catch (Exception)
                    {
                        return new JsonResult("ok");
                        throw;
                    }
                }
            }
        }
        [HttpPost]
        public JsonResult AddOrUpdate(int id, string positionName)
        {
            if (id == 0)
            {
                Position p = new Position
                {
                    Name = positionName
                };
                using (var conn = _dbContextFactory.CreateDbContext())
                {
                    try
                    {
                        conn.Positions.Add(p);
                        conn.SaveChanges();
                        return Json(Cbx_position()
                        );
                    }
                    catch (Exception)
                    {
                        //var result = new { mess = "Can't add object" };
                        return new JsonResult(false);


                        throw;
                    }
                }
            }
            else
            {
                try
                {
                    using (var conn = _dbContextFactory.CreateDbContext())
                    {
                        var ObjectEdit = conn.Positions.Find(id);
                        if (ObjectEdit != null)
                        {
                            ObjectEdit.Name = positionName;
                            conn.Update(ObjectEdit);
                            conn.SaveChanges();
                        }
                        return new JsonResult(
                        Cbx_position()
                        );
                    }
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        data = "Can't update",
                        status = false
                    });
                    throw;
                }
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            using (var conn = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    var ObjectDelete = conn.Positions.Find(id);
                    if (ObjectDelete != null)
                    {
                        conn.Positions.Remove(ObjectDelete);
                        conn.SaveChanges();
                    }
                    return new JsonResult(Cbx_position());
                }
                catch (Exception)
                {
                    return new JsonResult(false);
                    throw;
                }
            }
        }





    }
}
