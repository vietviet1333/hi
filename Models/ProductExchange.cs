using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus_Manegement.Models
{
    [Table("ProductExchange")]
    public class ProductExchange
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int BillID { get; set; }
        [ForeignKey("BillID")]
        public Bill ProductExchangeBill { get; set; }
        public int QuantityChange { get; set; }
        public bool Status { get; set; }
        public DateTime? ExchangeDate { get; set; }
    }
}
