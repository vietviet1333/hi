using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using Nexus_Manegement.Models;

namespace Nexus_Manegement.Areas.Management.Dao
{
    public class EmployeeDao
    {
        private static EmployeeDao? instance = null;
        private EmployeeDao() { }

        internal static EmployeeDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EmployeeDao();
                }
                return instance;
            }
        }
        public List<Employee> Employees
        {
            get

            {
                NexusContext f = new NexusContext();
                var result = (from e in f.Employees
                              join p in f.Positions on e.Position equals p.ID

                              select new Employee()
                              {
                               ID=e.ID,
                               Name=e.Name,
                               Phone=e.Phone,
                               Email=e.Email,
                               Username=e.Username,
                               Password=e.Password,
                               Status=e.Status,
                               PositionName=p.Name

                              }).ToList();
                return result;
            }
        }
        public Employee profile(int  Id)
        {


            NexusContext f = new NexusContext();
            var result = f.Employees.Where(x => x.ID == Id)
                .Select(x => new Employee() { ID = x.ID, Name = x.Name, Phone = x.Phone, Email = x.Email,Username=x.Username }).FirstOrDefault();
            return result;


        }
        public void Updateprofile(int Id, string name, string phone, string username,string email)
        {
            NexusContext f = new NexusContext();
            Employee e = f.Employees.Find(Id);
            e.Name=name;
            e.Phone=phone;
            e.Username=username;
            e.Email=email;
            f.SaveChanges();

        }
        public void Updatepass(int id, string pass)
        {
            NexusContext f= new NexusContext();
            Employee e = f.Employees.Find(id);
            e.Password=pass;
            f.SaveChanges();
        }
        public void UpdateOTP(int id, int otp)
        {
            NexusContext f = new NexusContext();
            Employee e = f.Employees.Find(id);
            e.OTP = otp;
            f.SaveChanges();
        }
        public void Deleteotp(int id)
        {
            NexusContext f = new NexusContext();
            Employee e = f.Employees.Find(id);
            e.OTP =0;
            f.SaveChanges();
        }
    }
}
