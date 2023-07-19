using Nexus_Manegement.Areas.Management.ModelView;
using Nexus_Manegement.Models;

namespace Nexus_Manegement.Areas.Management.Dao
{
    public class ServicePlanDao
    {
        private static ServicePlanDao? instance = null;
        private ServicePlanDao() { }

        internal static ServicePlanDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ServicePlanDao();
                }
                return instance;
            }
        }
        public void Addserviceplan(string Name, decimal Price, string Description, bool Status, int CategoryID, string fileName, string sum)
        {
            NexusContext f = new NexusContext();
            ServicePlan s = new ServicePlan();
            s.Name = Name;
            s.Price = Price;
            s.Description = Description;
            s.Status = Status;
            s.CategoryID = CategoryID;
            s.Image = fileName;
            s.Options = sum;
            f.Add(s);
            f.SaveChanges();

        }
        public List<ServiceplanView> ser
        {
            get 
            {
                NexusContext f = new NexusContext();
                var result = (from s in f.ServicePlans
                              join c in f.Categories on s.CategoryID equals c.ID
                              select new ServiceplanView()
                              {
                                  ID = s.ID,
                                  Name = s.Name,
                                  Price = s.Price,
                                  Description = s.Description,
                                  Status = s.Status,
                                  Namecategory = c.Name,
                                  Image = s.Image,
                                  Options = s.Options,

                              }).OrderByDescending(s=>s.ID).ToList();
                return result;

            }
        }
        public void Updateser(int Id, string Name, decimal Price, string Description, int CategoryID, string fileName, string sum)
        {
            NexusContext f = new NexusContext();
            ServicePlan s = f.ServicePlans.Find(Id);
            s.Name= Name;
            s.Price = Price;
            s.Description = Description;
            s.CategoryID = CategoryID;
            s.Image = fileName;
            s.Options = sum;
         
            f.SaveChanges();
        }
        public ServicePlan Editser(int Id)
        {

            NexusContext f = new NexusContext();
            var result = f.ServicePlans.Where(x => x.ID == Id).Select(x => new ServicePlan() 
            { ID = x.ID, 
              Name = x.Name,
              Price=x.Price,
              Description=x.Description ,CategoryID=x.CategoryID,Image=x.Image,Options=x.Options}).FirstOrDefault();
            return result;

        }
        public void Activeser(int Id)
        {
            NexusContext f= new NexusContext();
            ServicePlan s= f.ServicePlans.Find(Id);
            s.Status = true;
            f.SaveChanges();
        }
        public void Disactiveser(int Id)
        {
            NexusContext f= new NexusContext();
            ServicePlan s= f.ServicePlans.Find(Id);
            s.Status= false;
            f.SaveChanges();
        }
    }
}
