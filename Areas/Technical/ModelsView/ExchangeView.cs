using Nexus_Manegement.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus_Manegement.Areas.Technical.ModelsView
{
    public class ExchangeView
    {
        public int ID { get; set; }
        public int BillID { get; set; }
        public string? CustomerName { get; set; }
        public int Quantity { get; set; }
        public int QuantityChange { get; set; }
        public string? UsagePack { get; set; }
        public string? ServicePlanName { get; set; }
        public bool Status { get; set; }
        public string? ExchangeDate { get; set; }
    }
}
