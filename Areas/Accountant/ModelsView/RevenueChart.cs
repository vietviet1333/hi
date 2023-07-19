namespace Nexus_Management.Areas.Accountant.ModelsView
{
    public class RevenueChart
    {
        public string? MonthDate { get; set; }
        public decimal SumRevenue { get; set; }
        public int CountOrder { get; set; }
        public string? NameCategory { get; set; }
    }
}
