using Nexus_Manegement.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus_Management.Areas.Technical.ModelsView
{
    public class OrderView
    {
        public int IDOrder { get; set; }
        public int IDBill { get; set; }
        public int CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public string? MailCustomer { get; set; }
        public int ServicePlanID { get; set; }
        public string? ServiceName { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
        public int UsagePackID { get; set; }
        public string? UsagePack { get; set; }
        public decimal Total { get; set; }

        public string? OrderDate { get; set; }
        public string? OrderYear { get; set; }
        public int StatusService { get; set; }
        public string? RegistrationDate { get; set; }
        public string? ConnectionDate { get; set; }
        public string? ExpirationDate { get; set; }


    }
}
