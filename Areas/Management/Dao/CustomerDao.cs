using Nexus_Manegement.Models;

namespace Nexus_Manegement.Areas.Management.Dao
{
    public class CustomerDao
    {
        private static CustomerDao? instance = null;
        private CustomerDao() { }

        internal static CustomerDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomerDao();
                }
                return instance;
            }
        }
        public List<Customer> Customers
        {
            get

            {
                NexusContext f = new NexusContext();
                var result = f.Customers.Select(x => new Customer() { ID = x.ID, Name = x.Name,  Phone = x.Phone, Email = x.Email, Password = x.Password }).ToList();
                return result;
            }

        }
        //public Customer SeacherCustomer(string phone)
        //{


        //    NexusContext f = new NexusContext();
        //    var result = f.Customers.Where(x => x.Phone == phone)
        //        .Select(x => new Customer() { ID = x.ID, Name = x.Name, CitizenIDNo = x.CitizenIDNo, Phone = x.Phone, Email = x.Email, Password = x.Password }).FirstOrDefault();
        //    return result;


        //}
    }
}
