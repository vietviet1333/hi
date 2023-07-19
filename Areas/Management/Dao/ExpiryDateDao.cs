using Nexus_Manegement.Models;
using NuGet.Protocol;

namespace Nexus_Manegement.Areas.Management.Dao
{
    public class ExpiryDateDao
    {
        private static ExpiryDateDao? instance = null;
        private ExpiryDateDao() { }

        internal static ExpiryDateDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ExpiryDateDao();
                }
                return instance;
            }
        }
        public List<ExpiryDate> ExpiryDates
        {
            get
            {
                NexusContext f= new NexusContext();
                var result= f.ExpiryDate.Select(x=> new ExpiryDate() { ID=x.ID, Name=x.Name}).ToList();
                return result;
            }
        }
    }
}
