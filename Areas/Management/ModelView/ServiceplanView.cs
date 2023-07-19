using Nexus_Manegement.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus_Manegement.Areas.Management.ModelView
{
    public class ServiceplanView
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category ServicePlanCategory { get; set; }
        public string? Options { get; set; }

        public string Image { get; set; }
        public string Namecategory { get; set; }
    }
}
