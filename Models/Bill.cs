using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus_Manegement.Models
{
    [Table("Bill")]
    public class Bill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public int ID { get; set; }
        public string? IDPayment { get; set; }
        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public Order? BillOrder { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? ConnectionDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

    }
}
