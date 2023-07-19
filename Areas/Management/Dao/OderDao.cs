using Microsoft.Identity.Client;
using Nexus_Manegement.Areas.Management.ModelView;
using Nexus_Manegement.Models;

namespace Nexus_Manegement.Areas.Management.Dao
{
    public class OderDao
    {
        private static OderDao? instance = null;
        private OderDao() { }

        internal static OderDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OderDao();
                }
                return instance;
            }
        }
        public List<OderView>oder(int idc)
        {
          
                NexusContext f = new NexusContext();
                var result = (from o in f.Orders
                              join s in f.ServicePlans on o.ServicePlanID equals s.ID
                              join e in f.ExpiryDate on o.ExpiryDateID equals e.ID
                              where(o.CustomerID==idc)
                              select new OderView()
                              {
                              NameServicePlan=s.Name,
                              Oderdate=o.OrderDate,
                              NameExpiryDate=e.Name

                              }).ToList();
                return result;
            
        }
    }
}
