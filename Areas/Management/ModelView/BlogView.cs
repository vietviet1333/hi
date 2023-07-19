using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus_Manegement.Areas.Management.ModelView
{
    public class BlogView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }


    }
}
