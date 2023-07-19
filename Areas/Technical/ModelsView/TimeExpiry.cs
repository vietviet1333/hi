namespace Nexus_Manegement.Areas.Technical.ModelsView
{
    public class TimeExpiry
    {
        public int IDOrder { get; set; }
        public int IDBill { get; set; }
        public int CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public string? MailCustomer { get; set; }
        public int ServicePlanID { get; set; }
        public string? ServiceName { get; set; }
        public string? ConnectionDate { get; set; }
        public string? ExpirationDate { get; set; }
        public int? TimeSpan { get; set; }
    }
}
