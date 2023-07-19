using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus_Manegement.Models
{
    [Table("ServicePlan")]
    public class ServicePlan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category? ServicePlanCategory { get; set; }
        public string? Options { get; set; }
     
        public string Image { get; set; }
    }
}
