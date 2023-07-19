using Nexus_Manegement.Models;

namespace Nexus_Manegement.Areas.Management.Dao
{
    public class CategoryDao
    {

        private static CategoryDao? instance = null;
        private CategoryDao() { }

        internal static CategoryDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CategoryDao();
                }
                return instance;
            }
        }
        public List<Category> Categories
        {
            get
            {
                NexusContext f= new NexusContext();
                var result= f.Categories.Select(x=>new Category { ID=x.ID, Name=x.Name,}).ToList();
                return result;
            }
        }
    }
}
