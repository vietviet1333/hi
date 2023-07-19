using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus_Manegement.Areas.Management.ModelView
{
    public class OderView
    {
        public int ServicePlanId { get; set; }
        public int CustomerId { get; set; }
        public int ExpiryDateID { get; set; }
        [ForeignKey("ExpiryDateID")]
        public string NameServicePlan { get; set; }
        public DateTime Oderdate { get; set; }
        public string NameExpiryDate { get; set; }

    }
}
