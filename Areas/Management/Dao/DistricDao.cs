using Nexus_Manegement.Models;

namespace Nexus_Manegement.Areas.Management.Dao
{
    public class DistricDao
    {

        private static DistricDao? instance = null;
        private DistricDao() { }

        internal static DistricDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DistricDao();
                }
                return instance;
            }
        }
        public List<District> Districts
        {
            get
            {
                NexusContext f= new NexusContext();
                var result= f.Districts.Select(x=>new District { ID=x.ID, Name=x.Name,}).ToList();
                return result;
            }
        }
    }
}
